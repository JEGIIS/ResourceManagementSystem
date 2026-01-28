using CompanyResources.Shared;
using Microsoft.EntityFrameworkCore;

namespace CompanyResources.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Resource> Resources { get; set; }
        public DbSet<User> Users { get; set; } // Nowa tabela
    }
}
