using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using plv.Models;

namespace plv.ViewModels
{
    public class ManageRoleViewModel
    {
        public IEnumerable<IdentityRole> Role { get; set; }
    }
}
