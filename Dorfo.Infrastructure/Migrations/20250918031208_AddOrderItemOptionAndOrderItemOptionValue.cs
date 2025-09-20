using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dorfo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemOptionAndOrderItemOptionValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderItemOptions",
                columns: table => new
                {
                    OrderItemOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuItemOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptionName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemOptions", x => x.OrderItemOptionId);
                    table.ForeignKey(
                        name: "FK_OrderItemOptions_MenuItemOptions_MenuItemOptionId",
                        column: x => x.MenuItemOptionId,
                        principalTable: "MenuItemOptions",
                        principalColumn: "OptionId");
                    table.ForeignKey(
                        name: "FK_OrderItemOptions_OrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderItems",
                        principalColumn: "OrderItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemOptionValues",
                columns: table => new
                {
                    OrderItemOptionValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderItemOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuItemOptionValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceDelta = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemOptionValues", x => x.OrderItemOptionValueId);
                    table.ForeignKey(
                        name: "FK_OrderItemOptionValues_MenuItemOptionValues_MenuItemOptionValueId",
                        column: x => x.MenuItemOptionValueId,
                        principalTable: "MenuItemOptionValues",
                        principalColumn: "OptionValueId");
                    table.ForeignKey(
                        name: "FK_OrderItemOptionValues_OrderItemOptions_OrderItemOptionId",
                        column: x => x.OrderItemOptionId,
                        principalTable: "OrderItemOptions",
                        principalColumn: "OrderItemOptionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemOptions_MenuItemOptionId",
                table: "OrderItemOptions",
                column: "MenuItemOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemOptions_OrderItemId",
                table: "OrderItemOptions",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemOptionValues_MenuItemOptionValueId",
                table: "OrderItemOptionValues",
                column: "MenuItemOptionValueId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemOptionValues_OrderItemOptionId",
                table: "OrderItemOptionValues",
                column: "OrderItemOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItemOptionValues");

            migrationBuilder.DropTable(
                name: "OrderItemOptions");
        }
    }
}
