using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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
                    MerchantCategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantCategories", x => x.MerchantCategoryId);
                });

            // Update bảng Merchant: thêm cột MerchantCategoryId
            migrationBuilder.AddColumn<int>(
                name: "MerchantCategoryId",
                table: "Merchants",
                type: "integer",
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
