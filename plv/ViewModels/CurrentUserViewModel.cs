using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using plv.Models;

namespace plv.ViewModels  //Home/Index.cshtml
{
    public class CurrentUserViewModel : ApplicationUser
    {
        public List<string> Roles { get; set; }
    }
}
