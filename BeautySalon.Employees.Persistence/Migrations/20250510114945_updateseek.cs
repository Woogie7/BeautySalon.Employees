using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySalon.Employees.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateseek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_CustomDateOfWeeks_DateOfWeekId",
                table: "Schedules");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Employees",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_CustomDateOfWeeks_DateOfWeekId",
                table: "Schedules",
                column: "DateOfWeekId",
                principalTable: "CustomDateOfWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_CustomDateOfWeeks_DateOfWeekId",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Employees");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_CustomDateOfWeeks_DateOfWeekId",
                table: "Schedules",
                column: "DateOfWeekId",
                principalTable: "CustomDateOfWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
