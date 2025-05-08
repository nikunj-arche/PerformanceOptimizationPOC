using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PerformanceOptimizationApi.Data;
using PerformanceOptimizationApi.DTO;
using PerformanceOptimizationApi.Models;
using System.Collections.Concurrent;

namespace PerformanceOptimizationApi.Services
{
    public class ProductService
    {
        //private readonly ApplicationDbContext _context;
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, List<Product>> _cacheDict = new();
        private readonly string _cacheKey = "productListCache";

        //public ProductService(ApplicationDbContext context, IMemoryCache cache)
        //{
        //    _context = context;
        //    _cache = cache;
        //}
        public ProductService(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // Synchronous Method (Blocking)
        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        // Asynchronous Method (Non-blocking)
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.AsNoTracking()
                                          .ToListAsync()
                                          .ConfigureAwait(false);
        }

        // Slow LINQ Query (No Caching)
        public List<Product> GetProductsWithSlowLinq()
        {
            return _context.Products
               .AsNoTracking()
               .Where(p => p.Price > 10)
               .OrderBy(p => p.Id)
               .ToList();
        }

        // Optimized LINQ Query (With Caching)
        public List<Product> GetProductsWithCaching()
        {
            if (!_cache.TryGetValue(_cacheKey, out List<Product> cachedProducts))
            {
                cachedProducts = _context.Products
                    .AsNoTracking()
                    .Where(p => p.Price > 10)
                    .OrderBy(p => p.Id)
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                _cache.Set(_cacheKey, cachedProducts, cacheOptions);
            }

            return cachedProducts!;
        }

        // Optimized LINQ Query (With Select)
        //public List<Product> GetProductsWithOptimizedLinq()
        //{
        //    return _context.Products.Where(p => p.Price > 10).Select(p => new Product
        //    {
        //        Id = p.Id,
        //        Name = p.Name
        //    }).ToList();
        //}

        public List<Product> GetProductsWithOptimizedLinq()
        {
            return _context.Products.AsNoTracking()
                .Where(p => p.Price > 10)
                .OrderBy(p => p.Id)
                .ToList();
        }

        // Unoptimized LINQ Query
        public List<Product> GetProductsWithUnoptimizedLinq()
        {
            return _context.Products.ToList();  // Fetches all records and columns, unoptimized
        }

        public void RefreshCache()
        {
            var updatedProducts = _context.Products
                .AsNoTracking()
                .Where(p => p.Price > 10)
                .OrderBy(p => p.Id)
                .ToList();

            _cache.Set(_cacheKey, updatedProducts,
                new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5)));
        }
    }
}
