using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PerformanceOptimizationApi.Data;
using PerformanceOptimizationApi;
using PerformanceOptimizationApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerformanceOptimizationApi.Models;
using PerformanceOptimizationApi.DTO;

namespace BenchmarkRunnerApp
{
    [SimpleJob(launchCount: 1, warmupCount: 3, iterationCount: 10)]
    [MemoryDiagnoser]
    public class BenchmarkTests
    {
        private ProductService _service;

        [GlobalSetup]
        public void Setup()
        {
            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //    .UseInMemoryDatabase("BenchmarkDb")
            //    .Options;

            //var context = new ApplicationDbContext(options);
            //DataSeeder.SeedData(context);

            //// Set up your database context to point to SQLite (same configuration as in your web API)
            //var options = new DbContextOptionsBuilder<AppDbContext>()
            //              .UseSqlite("Data Source=app.db") // Point to your SQLite database
            //              .Options;

            //var context = new AppDbContext(options);

            //// Ensure the database is created and seed data
            //context.Database.EnsureCreated();  // Ensure the database is created if it doesn't exist

            var options = new DbContextOptionsBuilder<AppDbContext>()
              .UseSqlServer("Server=.\\SQLEXPRESS;Database=ProductDB;User ID=sa;Password=123;TrustServerCertificate=True;")
              .Options;
            var context = new AppDbContext(options);

            var cache = new MemoryCache(new MemoryCacheOptions());
            _service = new ProductService(context, cache);

            // Pre-warm the cache before benchmarking to avoid cache misses in the benchmarking tests
            _service.GetProductsWithCaching();
        }

        [Benchmark] public List<Product> GetProductsSync() => _service.GetProducts();
        [Benchmark] public async Task<List<Product>> GetProductsAsync() => await _service.GetProductsAsync().ConfigureAwait(false);
        [Benchmark] public List<Product> GetProductsWithSlowLinq() => _service.GetProductsWithSlowLinq();
        [Benchmark] public List<Product> GetProductsWithCaching() => _service.GetProductsWithCaching();
        [Benchmark] public List<Product> GetProductsWithUnoptimizedLinq() => _service.GetProductsWithUnoptimizedLinq();
        [Benchmark] public List<Product> GetProductsWithOptimizedLinq() => _service.GetProductsWithOptimizedLinq();
    }
}
