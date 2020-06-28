using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace plv.Models
{
    public class Section
    {
        public string Id { get; set; }

        [Display(Name = "Section Name: ")]
        [Required(ErrorMessage = "Enter section name")]
        [StringLength(30)]
        public string Name { get; set; }
    }
}
