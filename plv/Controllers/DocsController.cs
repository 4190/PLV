using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using plv.Models;
using plv.Data;
using plv.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.Differencing;

namespace plv.Controllers
{
    [Authorize(Roles = "User, Admin")]
    public class DocsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DocsController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext _context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            this._context = _context;
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        [Route("Docs/Create/{sectionName}")]
        public IActionResult Create(string sectionName)
        {
            ApplicationUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if(!_userManager.IsInRoleAsync(currentUser, sectionName + "-upload").Result)
            {
                return RedirectToAction("DocSections");
            }

            UploadFileViewModel viewModel = new UploadFileViewModel()
            {
                Section = sectionName
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(UploadFileViewModel model)
        {
            string selectedSectionName = model.Section;
            if (String.IsNullOrEmpty(selectedSectionName))
            {
                model.Success = false;
                model.LogMessage = "Choose section";
                return RedirectToAction("Create");
            }

            if (model.File != null)
            {
                string fileName = GetUniqueFileName(model.File.FileName);

                CreateUploadDirectoryIfDoesNotExist();
                CreateSectionDirectoryIfDoesNotExist(selectedSectionName);

                string savePath = Path.Combine(Directory.GetCurrentDirectory(),
                    $"wwwroot/uploads/{selectedSectionName}",
                    fileName);

                if (Path.GetExtension(savePath) != ".pdf")
                {
                    model.Success = false;
                    model.LogMessage = "Plik musi być plikiem .pdf";
                }
                else
                {
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        if(model.Sender == null)
                        {
                            model.Sender = "";
                        }
                        else if(model.Receiver == null)
                        {
                            model.Receiver = "";
                        }
                        model.File.CopyTo(stream); model.Success = true;
                        model.LogMessage = "Doc added to database";
                        SaveDocumentToDB(fileName, selectedSectionName, model);
                    }
                }
            }
            return View("Create", model);
        }

        [Route("Docs/Details/{id}")]
        public IActionResult Details(int id)
        {
            DocumentInDB doc = _context.Documents.Find(id);

            DocumentDetailsViewModel viewModel = new DocumentDetailsViewModel
            {
                Document = doc,
                IsOwnedByCurrentUser = (doc.CurrentUser == User.Identity.Name) ? true : false
            };


            return View(viewModel);
        }

        [Route("Docs/Download/{sectionName}/{filename}")]
        public IActionResult DownloadDocument(string sectionName, string filename)
        {
            if (filename == null)
                return Content("filename not present");

            string currentUserName = User.Identity.Name;

            ApplicationUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (!_userManager.IsInRoleAsync(currentUser, sectionName + "-download").Result && !_userManager.IsInRoleAsync(currentUser, "Admin").Result)
            {
                return Content("you can't download in this section");
            }
            else
            {
                try
                {
                    var path = Path.Combine(
                                   Directory.GetCurrentDirectory(),
                                   $"wwwroot/uploads/{sectionName}", filename);
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(path, FileMode.Open)) { stream.CopyTo(memory); }
                    memory.Position = 0;
                    SaveDownloadLog(currentUserName, sectionName, filename);
                    return File(memory, GetContentType(path), Path.GetFileName(path));  
                }
                catch (Exception e)
                {
                    return NotFound();
                }
            }
        }

        [Route("Docs/DocsList/{sectionName}")]
        public IActionResult DocsList(string sectionName)
        {
            ApplicationUser currentUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (!_userManager.IsInRoleAsync(currentUser, sectionName).Result && !_userManager.IsInRoleAsync(currentUser, "Admin").Result)
            {
                return Content("you can't browse in this section");
            }
            
            List<DocumentInDB> docs = _context.Documents.Where(c => c.Section == $"{sectionName}").ToList();

            DocumentListViewModel viewModel = new DocumentListViewModel
            {
                Documents = docs,
                CurrentSection = sectionName
            };

            return View(viewModel);
        }

        public IActionResult DocSections()
        {
            var sections = _context.Sections.ToList();
            return View(sections);
        }

        [Route("Docs/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            DocumentInDB doc = _context.Documents.Find(id);

            List<ApplicationUser> currentSectionUsers = GetCurrentSectionUsers(doc.Section);
            EditDocumentViewModel viewModel = new EditDocumentViewModel
            {
                Document = doc,
                Users = currentSectionUsers
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditDoc(EditDocumentViewModel model)
        {
            string currentUserName = User.Identity.Name;
            var docInDB = _context.Documents.Single(c => c.Id == model.Document.Id);

            SaveDocEdits(currentUserName, docInDB, model);


            if (!String.IsNullOrEmpty(model.Document.Receiver))
            {
                docInDB.Receiver = model.Document.Receiver;
            }
            if (!String.IsNullOrEmpty(model.Document.Sender))
            {
                docInDB.Sender = model.Document.Sender;
            }   
            docInDB.ShortOptionalDescription = model.Document.ShortOptionalDescription;
            docInDB.LastUser = docInDB.CurrentUser;
            docInDB.CurrentUser = model.Document.CurrentUser;
            if(model.Document.DateReceived != null && model.Document.DateReceived != DateTime.MinValue)
            {
                docInDB.DateReceived = model.Document.DateReceived;
            }

            
            _context.SaveChanges();

            return Redirect($"Details/{model.Document.Id}");
        }

#region HelperMethods

        private void CreateSectionDirectoryIfDoesNotExist(string sectionName)
        {
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/uploads/{sectionName}")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), 
                    $"wwwroot/uploads/{sectionName}"));
            }
        }

        private void CreateUploadDirectoryIfDoesNotExist()
        {
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads"));
            }
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private List<ApplicationUser> GetCurrentSectionUsers(string sectionName)
        {
            List<ApplicationUser> users = _context.Users.ToList();
            List<ApplicationUser> currentSectionUsers = new List<ApplicationUser>();

            foreach(var user in users)
            {
                if(_userManager.IsInRoleAsync(user, sectionName).Result)
                {
                    currentSectionUsers.Add(user);
                }
            }

            return currentSectionUsers;
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".pdf", "application/pdf"}
                /*
                {".txt", "text/plain"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},  
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
                */
            };
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 6)
                      + Path.GetExtension(fileName);
        }

        private void SaveDocEdits(string currentUserName, 
            DocumentInDB oldDocState, 
            EditDocumentViewModel newDocState)
        {
            DocEdits edits = new DocEdits();

            edits.EditedBy = currentUserName;
            if(oldDocState.DateReceived != newDocState.Document.DateReceived)
            {
                edits.NewDateIssued = newDocState.Document.DateReceived;
            }
            if(oldDocState.CurrentUser != newDocState.Document.CurrentUser)
            {
                edits.PreviousUser = oldDocState.CurrentUser;
                edits.NewUser = newDocState.Document.CurrentUser;
            }
            if(oldDocState.Receiver != newDocState.Document.Receiver)
            {
                edits.PreviousReceiver = oldDocState.Receiver;
                edits.NewReceiver = newDocState.Document.Receiver;
            }
            if(oldDocState.Sender != newDocState.Document.Sender)
            {
                edits.PreviousSender = oldDocState.Sender;
                edits.NewSender = newDocState.Document.Sender;
            }
            if(oldDocState.ShortOptionalDescription != newDocState.Document.ShortOptionalDescription)
            {
                edits.PreviousDescription = oldDocState.ShortOptionalDescription;
                edits.NewDescription = newDocState.Document.ShortOptionalDescription;
            }
            if(edits.NewDateIssued == null)
            {
                edits.NewDateIssued = DateTime.MinValue;
            }

            _context.DocumentEdits.Add(edits); _context.SaveChanges();
        }

        private void SaveDocumentToDB(string fileName, string selectedSectionName, UploadFileViewModel model)
        {
            DocumentInDB doc = new DocumentInDB
            {
                FilePath = fileName,
                Section = selectedSectionName,
                AddedBy = User.Identity.Name,
                CurrentUser = User.Identity.Name,
                Receiver = model.Receiver,
                Sender = model.Sender,
                ShortOptionalDescription = model.ShortOptionalDescription,
                DateAdded = DateTime.Now,
                DateReceived = model.DateReceived
            };
            DocumentsSection docSection = new DocumentsSection
            {
                DocumentInDB = doc,
                Section = _context.Sections.Find(model.SelectedSectionGuid)
            };
            _context.Documents.Add(doc);
            _context.DocumentsSections.Add(docSection);
            _context.SaveChanges();
        }

        private void SaveDownloadLog(string userName, string sectionName, string fileName)
        {
            Downloads _ = new Downloads
            {
                UserName = userName,
                SectionName = sectionName,
                FileName = fileName
            };

            _context.Add(_); _context.SaveChanges();
        }
#endregion
    }
}