using Microsoft.EntityFrameworkCore;
using Web_153502_Logvinovich.Domain.Entities;

namespace Web_153502_Logvinovich.Api.Data
{
    public class ApplicationDbContext: DbContext
    {

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}