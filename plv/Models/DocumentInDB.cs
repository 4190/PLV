using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace plv.Models
{
    public class DocumentInDB
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Section { get; set; }

        public string AddedBy { get; set; }
        public string CurrentUser { get; set; }
        public string LastUser { get; set; }
        [Required]
        [Display(Name= "Odbiorca: ")]
        public string Receiver { get; set; }
        [Required]
        [Display(Name= "Nadawca: ")]
        public string Sender { get; set; }
        [Display(Name= "Krótki opis (opcjonalnie): ")]
        public string ShortOptionalDescription { get; set; }
        
        public DateTime DateAdded { get; set; }
        public DateTime DateReceived { get; set; }
    }
}
