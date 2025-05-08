using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PerformanceOptimizationApi.Data;
using PerformanceOptimizationApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseInMemoryDatabase("InMemoryDb"));  // Use in-memory database for demo

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlite("Data Source=app.db"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// No changes to the existing code are needed beyond ensuring the above using directives are present.dotnet add package Microsoft.EntityFrameworkCore.SqlServer
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate(); // Apply any pending migrations
}

//using (var scope = app.Services.CreateScope())
//{
//    var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
//    productService.GetProductsWithCaching();
//}

//Task.Run(async () =>
//{
//    using var scope = app.Services.CreateScope();
//    var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
//    productService.GetProductsWithCaching();  
//});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Seed data
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    DataSeeder.SeedData(context);
//}



app.Run();
