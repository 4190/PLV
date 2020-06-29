using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using plv.Models;

namespace plv.ViewModels
{
    public class DocumentDetailsViewModel
    {
        public DocumentInDB Document { get; set; }
        public bool IsOwnedByCurrentUser { get; set; }
        public List<string> InvalidBlocksFieldsList { get; set; }
    }
}
