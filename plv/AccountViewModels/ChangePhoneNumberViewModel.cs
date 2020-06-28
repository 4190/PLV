using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace plv.AccountViewModels
{
    public class ChangePhoneNumberViewModel
    {
        [Required(ErrorMessage = "Phone Number cannot be empty")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "Phone number must contain 9 digits")]
        [Display(Name = "Phone number: ")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public string ErrorMessage { get; set; }
    }
}
