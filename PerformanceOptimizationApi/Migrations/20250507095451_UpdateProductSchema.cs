using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PerformanceOptimizationApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Product_Price",
                table: "Products",
                column: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_Price",
                table: "Products");
        }
    }
}
