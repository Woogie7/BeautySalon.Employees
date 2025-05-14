using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BeautySalon.Employees.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeleteStatusAndAviability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Employees_EmployeeId",
                table: "Availabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeStatus_StatusId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeStatus");

            migrationBuilder.DropIndex(
                name: "IX_Employees_StatusId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Availabilities_EmployeeId",
                table: "Availabilities");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Employees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EmployeeStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "EmployeeStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Inactive" },
                    { 3, "OnVacation" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StatusId",
                table: "Employees",
                column: "StatusId");

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
                name: "FK_Employees_EmployeeStatus_StatusId",
                table: "Employees",
                column: "StatusId",
                principalTable: "EmployeeStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
