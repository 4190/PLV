using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using plv.Models;

namespace plv.ViewModels
{
    public class DocumentListViewModel
    {
        public List<DocumentInDB> Documents { get; set; }
        public string CurrentSection { get; set; }
    }
}
