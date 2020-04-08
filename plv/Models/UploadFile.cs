using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;


namespace plv.Models
{
    public class UploadFile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] FileBinary { get; set; }
    }
    

}
