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
            // Tạo bảng MerchantCategory
            migrationBuilder.CreateTable(
                name: "MerchantCategories",
                columns: table => new
                {
                    MerchantCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantCategories", x => x.MerchantCategoryId);
                });

            // Update bảng Merchant: thêm cột MerchantCategoryId
            migrationBuilder.AddColumn<int>(
                name: "MerchantCategoryId",
                table: "Merchants",
                type: "int",
                nullable: true);

            // Thêm FK từ Merchant → MerchantCategory
            migrationBuilder.CreateIndex(
                name: "IX_Merchants_MerchantCategoryId",
                table: "Merchants",
                column: "MerchantCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_MerchantCategories_MerchantCategoryId",
                table: "Merchants",
                column: "MerchantCategoryId",
                principalTable: "MerchantCategories",
                principalColumn: "MerchantCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa FK và cột trong bảng Merchant
            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_MerchantCategories_MerchantCategoryId",
                table: "Merchants");

            migrationBuilder.DropIndex(
                name: "IX_Merchants_MerchantCategoryId",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "MerchantCategoryId",
                table: "Merchants");

            // Xóa bảng MerchantCategories
            migrationBuilder.DropTable(
                name: "MerchantCategories");
        }
    }
}
