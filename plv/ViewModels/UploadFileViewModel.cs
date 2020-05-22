using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using plv.Models;

namespace plv.ViewModels
{
    public class UploadFileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "Choose file to upload")]
        public IFormFile File { get; set; }
        public string LogMessage { get; set; }
        public bool Success { get; set; }


        public List<Section> SectionList { get; set; }
        [Display(Name = "Section: ")]
        [Required(ErrorMessage = "Choose section")]
        public string SelectedSectionGuid { get; set; }
    }
}
