using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dorfo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMerchantSchemaV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MerchantOpeningDays",
                columns: table => new
                {
                    MerchantOpeningDayId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MerchantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    OpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantOpeningDays", x => x.MerchantOpeningDayId);
                    table.ForeignKey(
                        name: "FK_MerchantOpeningDays_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MerchantOpeningDays_MerchantId",
                table: "MerchantOpeningDays",
                column: "MerchantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MerchantOpeningDays");
        }
    }
}
