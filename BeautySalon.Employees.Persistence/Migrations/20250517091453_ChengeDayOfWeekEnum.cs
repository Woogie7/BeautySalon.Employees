using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeautySalon.Employees.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChengeDayOfWeekEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_CustomDateOfWeeks_DateOfWeekId",
                table: "Schedules");

            migrationBuilder.DropIndex(
                name: "IX_Schedules_DateOfWeekId",
                table: "Schedules");

            migrationBuilder.DeleteData(
                table: "CustomDateOfWeeks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CustomDateOfWeeks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CustomDateOfWeeks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CustomDateOfWeeks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CustomDateOfWeeks",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CustomDateOfWeeks",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CustomDateOfWeeks",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.AddColumn<string>(
                name: "DateOfWeek",
                table: "Schedules",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfWeek",
                table: "Schedules");

            migrationBuilder.InsertData(
                table: "CustomDateOfWeeks",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Monday" },
                    { 2, "Tuesday" },
                    { 3, "Wednesday" },
                    { 4, "Thursday" },
                    { 5, "Friday" },
                    { 6, "Saturday" },
                    { 7, "Sunday" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_DateOfWeekId",
                table: "Schedules",
                column: "DateOfWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_CustomDateOfWeeks_DateOfWeekId",
                table: "Schedules",
                column: "DateOfWeekId",
                principalTable: "CustomDateOfWeeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
