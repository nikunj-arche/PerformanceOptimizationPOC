using Microsoft.EntityFrameworkCore;

namespace PerformanceOptimizationApi.Models
{
    [Index(nameof(Price))] // Moved the Index attribute to the class level as it is only valid on class declarations.  
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
