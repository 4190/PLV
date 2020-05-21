using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using plv.Models;

namespace plv.Models
{
    public class DocumentsSection
    {
        public int Id { get; set; }
        public DocumentInDB DocumentInDB { get; set; }
        public int DocumentInDBId { get; set; }
        public Section Section { get; set; }
        public string SectionId { get; set; }
    }
}
