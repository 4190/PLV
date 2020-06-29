using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using plv.BlockModels;
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
            if (!_userManager.IsInRoleAsync(currentUser, sectionName + "-upload").Result)
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
                string fileName = GetUniqueFileName(model.File.FileName, model.Section);

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
                        if (model.Sender == null)
                        {
                            model.Sender = "";
                        }
                        else if (model.Receiver == null)
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

        [Route("Docs/DownloadHistory/{filename}")]
        [Authorize(Roles="Admin")]
        public IActionResult DownloadHistory(string filename)
        {
            List<Downloads> downloadLog = new List<Downloads>();
            if(filename == "all")
            {
                downloadLog = _context.Downloads.ToList();
            }
            else
            {
                downloadLog = _context.Downloads.Where(x => x.FileName == filename).ToList();
            }


            return View(downloadLog);
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
            else
            {
                docInDB.Receiver = "";
            }
            if (!String.IsNullOrEmpty(model.Document.Sender))
            {
                docInDB.Sender = model.Document.Sender;
            }
            else
            {
                docInDB.Sender = "";
            }
            docInDB.ShortOptionalDescription = model.Document.ShortOptionalDescription;
            docInDB.CurrentUser = model.Document.CurrentUser;
            if (model.Document.DateReceived != null && model.Document.DateReceived != DateTime.MinValue)
            {
                docInDB.DateReceived = model.Document.DateReceived;
            }


            _context.SaveChanges();

            UpdateFirstBlock(docInDB);

            return Redirect($"Details/{model.Document.Id}");
        }

        [Route("Docs/EditHistory/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditHistory(int? id)
        {
            List<DocEdits> history = new List<DocEdits>();
            if (id == null || id == 0 )
            {
                history = _context.DocumentEdits.ToList();
            }
            else
            {
                history = _context.DocumentEdits.Where(c => c.DocumentId == id).ToList();
            }
            

            return View(history);
        }

        #region HelperMethods

        private string CalculateHash(string rawData)
        {
            using (SHA256 sha256hash = SHA256.Create())
            {
                byte[] bytes = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

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

        private string GetUniqueFileName(string fileName, string sectionName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + sectionName
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

            edits.EditTime = DateTime.Now;
            edits.DocumentId = newDocState.Document.Id;
            edits.DocumentName = oldDocState.FilePath;

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

            SaveFirstBlock(doc);
        }
        
        
        
        private void SaveDownloadLog(string userName, string sectionName, string fileName)
        {
            Downloads _ = new Downloads
            {
                UserName = userName,
                SectionName = sectionName,
                FileName = fileName,
                DownloadTime = DateTime.Now
            };

            _context.Add(_); _context.SaveChanges();
        }


        #endregion
        #region BlockMethods
        private void SaveFirstBlock(DocumentInDB doc)
        {
            FirstBlock block1 = new FirstBlock()
            {
                PreviousDocIdHash = null,
                DocIdHash = CalculateHash(doc.Id.ToString()),

                PreviousAddedByHash = null,
                AddedByHash = CalculateHash(doc.AddedBy),

                PreviousCurrentUserHash = null,
                CurrentUserHash = CalculateHash(doc.CurrentUser),

                PreviousReceiverHash = null,
                ReceiverHash = CalculateHash(doc.Receiver),

                PreviousSenderHash = null,
                SenderHash = CalculateHash(doc.Sender),

                PreviousShortOptionalDescriptionHash = null,
                ShortOptionalDescriptionHash = CalculateHash(doc.ShortOptionalDescription),

                PreviousDateAddedHash = null,
                DateAddedHash = CalculateHash(doc.DateAdded.ToString()),

                PreviousDateReceivedHash = null,
                DateReceivedHash = CalculateHash(doc.DateReceived.ToString())
            };
            _context.FirstBlock.Add(block1); _context.SaveChanges();
            SaveSecondBlock(block1);
        }

        private void SaveSecondBlock(FirstBlock block1)
        {
            SecondBlock block2 = new SecondBlock()
            {
                PreviousDocIdHash = block1.DocIdHash,
                DocIdHash = CalculateHash(block1.DocIdHash),

                PreviousAddedByHash = block1.AddedByHash,
                AddedByHash = CalculateHash(block1.AddedByHash),

                PreviousCurrentUserHash = block1.CurrentUserHash,
                CurrentUserHash = CalculateHash(block1.CurrentUserHash),

                PreviousReceiverHash = block1.ReceiverHash,
                ReceiverHash = CalculateHash(block1.ReceiverHash),

                PreviousSenderHash = block1.SenderHash,
                SenderHash = CalculateHash(block1.SenderHash),

                PreviousShortOptionalDescriptionHash = block1.ShortOptionalDescriptionHash,
                ShortOptionalDescriptionHash = CalculateHash(block1.ShortOptionalDescriptionHash),

                PreviousDateAddedHash = block1.DateAddedHash,
                DateAddedHash = CalculateHash(block1.DateAddedHash),

                PreviousDateReceivedHash = block1.DateReceivedHash,
                DateReceivedHash = CalculateHash(block1.DateReceivedHash)
            };
            _context.SecondBlock.Add(block2); _context.SaveChanges();
            SaveThirdBlock(block2);
        }

        private void SaveThirdBlock(SecondBlock block2)
        {
            ThirdBlock block3 = new ThirdBlock()
            {
                PreviousDocIdHash = block2.DocIdHash,
                DocIdHash = CalculateHash(block2.DocIdHash),

                PreviousAddedByHash = block2.AddedByHash,
                AddedByHash = CalculateHash(block2.AddedByHash),

                PreviousCurrentUserHash = block2.CurrentUserHash,
                CurrentUserHash = CalculateHash(block2.CurrentUserHash),

                PreviousReceiverHash = block2.ReceiverHash,
                ReceiverHash = CalculateHash(block2.ReceiverHash),

                PreviousSenderHash = block2.SenderHash,
                SenderHash = CalculateHash(block2.SenderHash),

                PreviousShortOptionalDescriptionHash = block2.ShortOptionalDescriptionHash,
                ShortOptionalDescriptionHash = CalculateHash(block2.ShortOptionalDescriptionHash),

                PreviousDateAddedHash = block2.DateAddedHash,
                DateAddedHash = CalculateHash(block2.DateAddedHash),

                PreviousDateReceivedHash = block2.DateReceivedHash,
                DateReceivedHash = CalculateHash(block2.DateReceivedHash)
            };
            _context.ThirdBlock.Add(block3); _context.SaveChanges();
        }

        private void UpdateFirstBlock(DocumentInDB doc)
        {
            string firstBlockDocIdHash = CalculateHash(doc.Id.ToString());
            FirstBlock block1 = _context.FirstBlock.Where(x => x.DocIdHash == firstBlockDocIdHash).Single();

            block1.PreviousDocIdHash = null;
            block1.DocIdHash = CalculateHash(doc.Id.ToString());

            block1.PreviousAddedByHash = null;
            block1.AddedByHash = CalculateHash(doc.AddedBy);

            block1.PreviousCurrentUserHash = null;
            block1.CurrentUserHash = CalculateHash(doc.CurrentUser);

            block1.PreviousReceiverHash = null;
            block1.ReceiverHash = CalculateHash(doc.Receiver);

            block1.PreviousSenderHash = null;
            block1.SenderHash = CalculateHash(doc.Sender);

            block1.PreviousShortOptionalDescriptionHash = null;
            block1.ShortOptionalDescriptionHash = CalculateHash(doc.ShortOptionalDescription);

            block1.PreviousDateAddedHash = null;
            block1.DateAddedHash = CalculateHash(doc.DateAdded.ToString());

            block1.PreviousDateReceivedHash = null;
            block1.DateReceivedHash = CalculateHash(doc.DateReceived.ToString());

            _context.SaveChanges();
            UpdateSecondBlock(block1);
        }
        private void UpdateSecondBlock(FirstBlock block1)
        {
            SecondBlock block2 = _context.SecondBlock.Where(x => x.PreviousDocIdHash == block1.DocIdHash).Single();
            block2.PreviousDocIdHash = block1.DocIdHash;
            block2.DocIdHash = CalculateHash(block1.DocIdHash);

            block2.PreviousAddedByHash = block1.AddedByHash;
            block2.AddedByHash = CalculateHash(block1.AddedByHash);

            block2.PreviousCurrentUserHash = block1.CurrentUserHash;
            block2.CurrentUserHash = CalculateHash(block1.CurrentUserHash);

            block2.PreviousReceiverHash = block1.ReceiverHash;
            block2.ReceiverHash = CalculateHash(block1.ReceiverHash);

            block2.PreviousSenderHash = block1.SenderHash;
            block2.SenderHash = CalculateHash(block1.SenderHash);

            block2.PreviousShortOptionalDescriptionHash = block1.ShortOptionalDescriptionHash;
            block2.ShortOptionalDescriptionHash = CalculateHash(block1.ShortOptionalDescriptionHash);

            block2.PreviousDateAddedHash = block1.DateAddedHash;
            block2.DateAddedHash = CalculateHash(block1.DateAddedHash);

            block2.PreviousDateReceivedHash = block1.DateReceivedHash;
            block2.DateReceivedHash = CalculateHash(block1.DateReceivedHash);
            _context.SaveChanges();
            UpdateThirdBlock(block2);
        }

        private void UpdateThirdBlock(SecondBlock block2)
        {
            ThirdBlock block3 = _context.ThirdBlock.Where(x => x.PreviousDocIdHash == block2.DocIdHash).Single();
            block3.PreviousDocIdHash = block2.DocIdHash;
            block3.DocIdHash = CalculateHash(block2.DocIdHash);

            block3.PreviousAddedByHash = block2.AddedByHash;
            block3.AddedByHash = CalculateHash(block2.AddedByHash);

            block3.PreviousCurrentUserHash = block2.CurrentUserHash;
            block3.CurrentUserHash = CalculateHash(block2.CurrentUserHash);

            block3.PreviousReceiverHash = block2.ReceiverHash;
            block3.ReceiverHash = CalculateHash(block2.ReceiverHash);

            block3.PreviousSenderHash = block2.SenderHash;
            block3.SenderHash = CalculateHash(block2.SenderHash);

            block3.PreviousShortOptionalDescriptionHash = block2.ShortOptionalDescriptionHash;
            block3.ShortOptionalDescriptionHash = CalculateHash(block2.ShortOptionalDescriptionHash);

            block3.PreviousDateAddedHash = block2.DateAddedHash;
            block3.DateAddedHash = CalculateHash(block2.DateAddedHash);

            block3.PreviousDateReceivedHash = block2.DateReceivedHash;
            block3.DateReceivedHash = CalculateHash(block2.DateReceivedHash);
            _context.SaveChanges();
        }
        #endregion
    }
}