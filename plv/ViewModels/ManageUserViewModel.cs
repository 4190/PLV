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
        public List<string> CurrentUserRoles { get; set; }
        public IEnumerable<IdentityRole> RolesList { get; set; }
    }
}
