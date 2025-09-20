using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dorfo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMerchantSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "MerchantSettings");

            migrationBuilder.DropColumn(
                name: "FreeShipThreshold",
                table: "MerchantSettings");

            migrationBuilder.DropColumn(
                name: "MinOrderAmount",
                table: "MerchantSettings");

            migrationBuilder.DropColumn(
                name: "PrepWindowMinutes",
                table: "MerchantSettings");

            migrationBuilder.DropColumn(
                name: "FreeShipThreshold",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "MinOrderAmount",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "PrepWindowMinutes",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "SupportsScheduling",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "MerchantAddresses");

            migrationBuilder.DropColumn(
                name: "GeoLat",
                table: "MerchantAddresses");

            migrationBuilder.DropColumn(
                name: "GeoLng",
                table: "MerchantAddresses");

            migrationBuilder.RenameColumn(
                name: "Building",
                table: "MerchantAddresses",
                newName: "Ward");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "MerchantAddresses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "MerchantAddresses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "MerchantAddresses",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "MerchantAddresses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetNumber",
                table: "MerchantAddresses",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "MerchantAddresses");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "MerchantAddresses");

            migrationBuilder.DropColumn(
                name: "District",
                table: "MerchantAddresses");

            migrationBuilder.DropColumn(
                name: "StreetName",
                table: "MerchantAddresses");

            migrationBuilder.DropColumn(
                name: "StreetNumber",
                table: "MerchantAddresses");

            migrationBuilder.RenameColumn(
                name: "Ward",
                table: "MerchantAddresses",
                newName: "Building");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "MerchantSettings",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AddColumn<decimal>(
                name: "FreeShipThreshold",
                table: "MerchantSettings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinOrderAmount",
                table: "MerchantSettings",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrepWindowMinutes",
                table: "MerchantSettings",
                type: "int",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.AddColumn<decimal>(
                name: "FreeShipThreshold",
                table: "Merchants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinOrderAmount",
                table: "Merchants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrepWindowMinutes",
                table: "Merchants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SupportsScheduling",
                table: "Merchants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "MerchantAddresses",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GeoLat",
                table: "MerchantAddresses",
                type: "decimal(9,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GeoLng",
                table: "MerchantAddresses",
                type: "decimal(9,6)",
                nullable: true);
        }
    }
}
