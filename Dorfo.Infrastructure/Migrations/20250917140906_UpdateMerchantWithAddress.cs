using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dorfo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMerchantWithAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MerchantAddresses_MerchantId",
                table: "MerchantAddresses");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "MerchantAddresses");

            migrationBuilder.CreateIndex(
                name: "IX_MerchantAddresses_MerchantId",
                table: "MerchantAddresses",
                column: "MerchantId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MerchantAddresses_MerchantId",
                table: "MerchantAddresses");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "MerchantAddresses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MerchantAddresses_MerchantId",
                table: "MerchantAddresses",
                column: "MerchantId");
        }
    }
}
