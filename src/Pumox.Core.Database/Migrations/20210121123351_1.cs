#region using

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#endregion

namespace Pumox.Core.Database.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                "pcd");

            migrationBuilder.CreateTable(
                "Company",
                schema: "pcd",
                columns: table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>("varchar(64)", maxLength: 64, nullable: false),
                    EstablishmentYear = table.Column<short>("smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Employee",
                schema: "pcd",
                columns: table => new
                {
                    Id = table.Column<long>("bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>("bigint", nullable: false),
                    FirstName = table.Column<string>("varchar(64)", maxLength: 64, nullable: false),
                    LastName = table.Column<string>("varchar(64)", maxLength: 64, nullable: false),
                    DateOfBirth = table.Column<DateTime>("datetime2", nullable: false),
                    JobTitle = table.Column<byte>("tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        "FK_Employee_Company_CompanyId",
                        x => x.CompanyId,
                        principalSchema: "pcd",
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Company_EstablishmentYear",
                schema: "pcd",
                table: "Company",
                column: "EstablishmentYear");

            migrationBuilder.CreateIndex(
                "IX_Company_Name",
                schema: "pcd",
                table: "Company",
                column: "Name");

            migrationBuilder.CreateIndex(
                "IX_Employee_CompanyId",
                schema: "pcd",
                table: "Employee",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                "IX_Employee_DateOfBirth",
                schema: "pcd",
                table: "Employee",
                column: "DateOfBirth");

            migrationBuilder.CreateIndex(
                "IX_Employee_FirstName",
                schema: "pcd",
                table: "Employee",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                "IX_Employee_JobTitle",
                schema: "pcd",
                table: "Employee",
                column: "JobTitle");

            migrationBuilder.CreateIndex(
                "IX_Employee_LastName",
                schema: "pcd",
                table: "Employee",
                column: "LastName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Employee",
                "pcd");

            migrationBuilder.DropTable(
                "Company",
                "pcd");
        }
    }
}
