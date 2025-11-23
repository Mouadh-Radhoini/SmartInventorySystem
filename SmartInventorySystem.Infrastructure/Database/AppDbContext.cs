using Microsoft.EntityFrameworkCore;
using SmartInventorySystem.Domain.Entities;

namespace SmartInventorySystem.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    }
}