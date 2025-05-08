using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerformanceOptimizationApi.Services;

namespace PerformanceOptimizationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("sync")]
        public IActionResult GetProductsSync()
        {
            var products = _productService.GetProducts();
            return Ok(products);
        }

        [HttpGet("async")]
        public async Task<IActionResult> GetProductsAsync()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("slow")]
        public IActionResult GetProductsWithSlowLinq()
        {
            var products = _productService.GetProductsWithSlowLinq();
            return Ok(products);
        }

        [HttpGet("cached")]
        public IActionResult GetProductsWithCache()
        {
            var products = _productService.GetProductsWithCaching();
            return Ok(products);
        }

        [HttpGet("optimized")]
        public IActionResult GetProductsWithOptimizedLinq()
        {
            var products = _productService.GetProductsWithOptimizedLinq();
            return Ok(products);
        }

        [HttpGet("unoptimized")]
        public IActionResult GetProductsWithUnoptimizedLinq()
        {
            var products = _productService.GetProductsWithUnoptimizedLinq();
            return Ok(products);
        }
    }
}
