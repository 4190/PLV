using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace plv.Models
{
    public class DocEdits
    {
        public int Id { get; set; }
        public string DocumentName { get; set; }
        public int DocumentId { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditTime { get; set; }


        public DateTime NewDateIssued { get; set; }
        public string PreviousUser { get; set; }
        public string NewUser { get; set; }
        public string PreviousReceiver { get; set; }
        public string NewReceiver { get; set; }
        public string PreviousSender { get; set; }
        public string NewSender { get; set; }
        public string PreviousDescription { get; set; }
        public string NewDescription { get; set; }
    }
}
