using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySalon.Employees.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Services_ServiceId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "IsBusy",
                table: "Availabilities");

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_EmployeeId",
                table: "Availabilities",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Employees_EmployeeId",
                table: "Availabilities",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Services_ServiceId",
                table: "Skills",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Employees_EmployeeId",
                table: "Availabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Services_ServiceId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Availabilities_EmployeeId",
                table: "Availabilities");

            migrationBuilder.AddColumn<bool>(
                name: "IsBusy",
                table: "Availabilities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Services_ServiceId",
                table: "Skills",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
