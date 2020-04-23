using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using plv.Models;

namespace plv.ViewModels
{
    public class ManageUserViewModel
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<IdentityRole> Role { get; set; }
        public string SelectedRoleId { get; set; }
    }
}
