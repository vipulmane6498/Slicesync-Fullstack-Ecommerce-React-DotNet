using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SliceSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCartPriceColumnInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CartPrice",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartPrice",
                table: "Carts");
        }
    }
}
