using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortener.DAL.EF
{
    public class URLShortenerDbContext : DbContext
    {
        public DbSet<URLInfo> URLInfos { get; set; }
        public URLShortenerDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<URLInfo>();

        }

    }
}
