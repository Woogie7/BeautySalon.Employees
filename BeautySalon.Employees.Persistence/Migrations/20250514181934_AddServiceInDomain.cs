using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautySalon.Employees.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceInDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Skills");

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "Skills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ServiceId",
                table: "Skills",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Services_ServiceId",
                table: "Skills",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Services_ServiceId",
                table: "Skills");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Skills_ServiceId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Skills");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Skills",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
