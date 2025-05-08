using PerformanceOptimizationApi.Data;
using PerformanceOptimizationApi.Models;

namespace PerformanceOptimizationApi
{
    public static class DataSeeder
    {
        public static void SeedData(ApplicationDbContext context)
        {
            var products = new List<Product>();

            // Adding 1000 products to show a more significant performance difference
            for (int i = 0; i < 10000; i++)
            {
                products.Add(new Product
                {
                    Name = $"Product {i + 1}",
                    Price = (i % 100) + 1  // Generate some variety in prices
                });
            }

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}
