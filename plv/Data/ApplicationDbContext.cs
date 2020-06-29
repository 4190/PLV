using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using plv.Models;
using plv.BlockModels;

namespace plv.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<DocumentInDB> Documents { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<DocumentsSection> DocumentsSections { get; set; }
        public DbSet<DocEdits> DocumentEdits { get; set; }
        public DbSet<Downloads> Downloads { get; set; }
        public DbSet<FirstBlock> FirstBlock { get; set; }
        public DbSet<SecondBlock> SecondBlock { get; set; }
        public DbSet<ThirdBlock> ThirdBlock { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
