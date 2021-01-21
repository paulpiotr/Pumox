using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pumox.Core.Database.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "pcd");

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "pcd",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "bigint identity(1,1)", nullable: false),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    EstablishmentYear = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                schema: "pcd",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "bigint identity(1,1)", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    LastName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JobTitle = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "pcd",
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_EstablishmentYear",
                schema: "pcd",
                table: "Company",
                column: "EstablishmentYear");

            migrationBuilder.CreateIndex(
                name: "IX_Company_Name",
                schema: "pcd",
                table: "Company",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CompanyId",
                schema: "pcd",
                table: "Employee",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DateOfBirth",
                schema: "pcd",
                table: "Employee",
                column: "DateOfBirth");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_FirstName",
                schema: "pcd",
                table: "Employee",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_JobTitle",
                schema: "pcd",
                table: "Employee",
                column: "JobTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_LastName",
                schema: "pcd",
                table: "Employee",
                column: "LastName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee",
                schema: "pcd");

            migrationBuilder.DropTable(
                name: "Company",
                schema: "pcd");
        }
    }
}
