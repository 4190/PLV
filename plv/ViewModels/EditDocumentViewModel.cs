using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using plv.Models;

namespace plv.ViewModels
{
    public class EditDocumentViewModel
    {
        public DocumentInDB Document { get; set; }

        public List<ApplicationUser> Users { get; set; }
    }
}
