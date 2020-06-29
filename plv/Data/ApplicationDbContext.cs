using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using plv.Models;

namespace plv.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<DocumentInDB> Documents { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<DocumentsSection> DocumentsSections { get; set; }
        public DbSet<DocEdits> DocumentEdits { get; set; }
        public DbSet<Downloads> Downloads { get; set; }
        public DbSet<Block> Block1 { get; set; }
        public DbSet<Block> Block2 { get; set; }
        public DbSet<Block> Block3 { get; set; }
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
