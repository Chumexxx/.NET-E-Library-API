using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModernLibrary.Models;
using System.Reflection.Emit;

namespace ModernLibrary.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {

        }

        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowingRecord> BorrowingRecords { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks{ get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithMany(b => b.Authors)
            .UsingEntity<Dictionary<string, object>>(
                "AuthorBook",
                ab => ab.HasOne<Book>().WithMany().HasForeignKey("BookId"),
                ab => ab.HasOne<Author>().WithMany().HasForeignKey("AuthorId"));

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                },
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                },
                new IdentityRole
                {
                    Name = "Staff",
                    NormalizedName = "STAFF"
                },
                new IdentityRole
                {
                    Name = "Librarian",
                    NormalizedName = "LIBRARIAN"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
