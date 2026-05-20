using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SliceSync.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class JwtRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JwtRefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "JwtRefreshTokenExpirationDateTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JwtRefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "JwtRefreshTokenExpirationDateTime",
                table: "AspNetUsers");
        }
    }
}
