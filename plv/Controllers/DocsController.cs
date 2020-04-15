using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
            return View();
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
        /*
        [HttpPost]
        public IActionResult Create(UploadFileViewModel model)
        {
            
            if(model.File != null)
            {
                var uniqueFileName = GetUniqueFileName(model.File.FileName);
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                var filePath = Path.Combine(uploads, uniqueFileName);
                model.File.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            return Content("ok");
        }
        */

        [HttpPost]  //TODO
        public IActionResult Create(UploadFileViewModel model)
        {
            if (model.File != null)
            {
                string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);

                string SavePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", ImageName);

                using (var stream = new FileStream(SavePath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }
            }
            return Content("k");
        }
    }
}