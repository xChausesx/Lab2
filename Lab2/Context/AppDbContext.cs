using Lab2.Areas.Identity.Data;
using Lab2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Context
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Keyword> Keywords{ get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();
            }
        }
    }
}
