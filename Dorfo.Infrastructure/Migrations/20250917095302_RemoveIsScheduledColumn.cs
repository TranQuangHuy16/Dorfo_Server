using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dorfo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsScheduledColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsScheduled",
                table: "Orders",
                type: "bit",
                nullable: false,
                computedColumnSql: "CAST(CASE WHEN [ScheduledFor] IS NULL THEN 0 ELSE 1 END AS BIT)",
                stored: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComputedColumnSql: "CASE WHEN [ScheduledFor] IS NULL THEN 0 ELSE 1 END",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsScheduled",
                table: "Orders",
                type: "bit",
                nullable: false,
                computedColumnSql: "CASE WHEN [ScheduledFor] IS NULL THEN 0 ELSE 1 END",
                stored: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldComputedColumnSql: "CAST(CASE WHEN [ScheduledFor] IS NULL THEN 0 ELSE 1 END AS BIT)",
                oldStored: true);
        }
    }
}
