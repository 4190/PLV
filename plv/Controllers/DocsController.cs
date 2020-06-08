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

        public IActionResult Create()
        {
            UploadFileViewModel viewModel = new UploadFileViewModel()
            {
                SectionList = _context.Sections.ToList()
            };
            return View(viewModel);
        }


        [HttpPost]
        public IActionResult Create(UploadFileViewModel model)
        {
            string selectedSectionName = _context.Sections.Find(model.SelectedSectionGuid).Name;
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
                        model.File.CopyTo(stream); model.Success = true;
                        model.LogMessage = "Doc added to database";
                        SaveDocumentToDB(fileName, selectedSectionName, model);
                    }
                }
            }
            model.SectionList = _context.Sections.ToList();
            return View(model);
        }

        [Route("Docs/Details/{id}")]
        public IActionResult Details(int id)
        {
            DocumentInDB doc = _context.Documents.Find(id);

            return View(doc);
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

            var docs = _context.Documents.Where(c => c.Section == $"{sectionName}").ToList();
            return View(docs);
        }

        public IActionResult DocSections()
        {
            var sections = _context.Sections.ToList();
            return View(sections);
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
#endregion
    }
}