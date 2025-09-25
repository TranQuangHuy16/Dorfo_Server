using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dorfo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Merchants",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "MenuItems",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "MenuItems");
        }
    }
}
