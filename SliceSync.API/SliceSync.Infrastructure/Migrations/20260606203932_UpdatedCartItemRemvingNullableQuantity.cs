using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SliceSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCartItemRemvingNullableQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quanitty",
                table: "CartItem");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CartItem",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CartItem");

            migrationBuilder.AddColumn<int>(
                name: "Quanitty",
                table: "CartItem",
                type: "int",
                nullable: true);
        }
    }
}
