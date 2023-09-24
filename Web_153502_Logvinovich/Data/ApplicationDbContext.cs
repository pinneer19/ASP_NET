using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_153502_Logvinovich.Domain.Entities;

namespace Web_153502_Logvinovich.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Web_153502_Logvinovich.Domain.Entities.Book> Book { get; set; } = default!;
    }
}