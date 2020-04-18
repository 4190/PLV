using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using plv.Models;
using plv.Data;
using plv.ViewModels;

namespace plv.Controllers
{
    public class DocsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public DocsController(ApplicationDbContext _context, IWebHostEnvironment environment)
        {
            hostingEnvironment = environment;
            this._context = _context;
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public IActionResult Create()
        {
            UploadFileViewModel viewModel = new UploadFileViewModel();

            return View(viewModel);
        }


        [HttpPost]  //TODO
        public IActionResult Create(UploadFileViewModel model)
        {
            if (model.File != null)
            {
                string fileName = GetUniqueFileName(model.File.FileName); //Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
                
                if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads"));
                }

                string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);  //Todo wwwroot/uploads/{sectionName}

                if (Path.GetExtension(savePath) != ".pdf")
                {
                    model.LogMessage = "Plik musi być plikiem .pdf";
                    model.Success = false;
                    model.File = null;
                    model.Name = null;
                }
                else
                {
                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        model.File.CopyTo(stream);
                        model.Success = true;
                        model.LogMessage = "Dokument dodany do bazy";

                        DocumentInDB doc = new DocumentInDB
                        {
                            FilePath = fileName,
                            Section = ""
                        };
                        _context.Documents.Add(doc); _context.SaveChanges();
                    }
                }
            }
            else
            {
                model.LogMessage = "Nie podano żadnego pliku do uploadu";
                model.Success = false;

            }
            model.File = null; model.Name = null;
            return View(model);
        }

        [Route("Docs/Download/{filename}")]
        public IActionResult DownloadDocument(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot/uploads", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open)) { stream.CopyTo(memory); }
            memory.Position = 0;

            return File(memory, GetContentType(path), Path.GetFileName(path));
        }


        public IActionResult DocsList()
        {
            var docs = _context.Documents.ToList();

            return View(docs);
        }


        #region HelperMethods
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
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        #endregion
    }
}