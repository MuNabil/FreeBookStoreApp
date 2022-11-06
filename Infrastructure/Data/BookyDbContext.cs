using Domain.Entities;
using Infrastructure.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class BookyDbContext : IdentityDbContext<ApplicationUser>
    {
        public BookyDbContext(DbContextOptions<BookyDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>().Property(c => c.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<LogCategory>().Property(c => c.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<SubCategory>().Property(c => c.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<LogSubCategory>().Property(c => c.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<Book>().Property(c => c.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<LogBook>().Property(c => c.Id).HasDefaultValueSql("NEWID()");
            builder.Entity<VmUser>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("VwUsers");
            });
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<LogCategory> LogCategories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<LogSubCategory> LogSubCategories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<LogBook> LogBooks { get; set; }
        public DbSet<VmUser> VwUsers { get; set; }
    }
}
