using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Mvc;
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

        public DocsController(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UploadFileViewModel model)
        {
            //Create an object of your entity class and map property values
            var post = new UploadFile{  Name = model.Name };

            if (model.File != null)
            {
                post.FileBinary = GetByteArrayFromFile(model.File);
            }
            _context.Files.Add(post);
            _context.SaveChanges();
            return RedirectToAction("Create", "Docs");

        }

        private byte[] GetByteArrayFromFile(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
    }
}