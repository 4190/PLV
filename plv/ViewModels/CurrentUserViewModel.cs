﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using plv.Models;

namespace plv.ViewModels  
{
    public class CurrentUserViewModel : ApplicationUser
    {
        public ApplicationUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
