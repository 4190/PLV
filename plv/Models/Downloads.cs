using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace plv.Models
{
    public class Downloads
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public int DocumentId { get; set; }
        public DateTime DownloadTime { get; set; }

        public string UserName { get; set; }
        public string SectionName { get; set; }
        public string FileName { get; set; }
    }
}
