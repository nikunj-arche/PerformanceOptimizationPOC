using Microsoft.EntityFrameworkCore;
using PerformanceOptimizationApi.Models;

namespace PerformanceOptimizationApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
    }
}

