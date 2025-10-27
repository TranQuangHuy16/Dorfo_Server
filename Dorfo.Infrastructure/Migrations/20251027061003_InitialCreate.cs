using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dorfo.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ✅ Thêm cột MerchantId vào bảng Reviews
            migrationBuilder.AddColumn<Guid>(
                name: "MerchantId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            // ✅ Tạo Index cho MerchantId
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MerchantId",
                table: "Reviews",
                column: "MerchantId");

            // ✅ Tạo Foreign Key nhưng KHÔNG cascade xoá
            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Merchants_MerchantId",
                table: "Reviews",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "MerchantId",
                onDelete: ReferentialAction.NoAction // 👈 chính chỗ này là fix
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Merchants_MerchantId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MerchantId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "MerchantId",
                table: "Reviews");
        }
    }
}
