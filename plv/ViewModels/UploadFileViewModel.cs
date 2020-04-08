﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace plv.ViewModels
{
    public class UploadFileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
