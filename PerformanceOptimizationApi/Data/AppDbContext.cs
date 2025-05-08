using Microsoft.EntityFrameworkCore;
using PerformanceOptimizationApi.Models;

namespace PerformanceOptimizationApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Price) 
                .HasDatabaseName("IX_Product_Price");
        }
    }
}
