using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClientUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_departments_DepartmentId",
                schema: "dms",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_departments_DepartmentId",
                schema: "dms",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentId",
                schema: "dms",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DepartmentId",
                schema: "dms",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "dms",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "dms",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentHeadId",
                schema: "dms",
                table: "departments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "dms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalSchema: "dms",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employee_departments",
                schema: "dms",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee_departments", x => new { x.EmployeeId, x.DepartmentId });
                    table.ForeignKey(
                        name: "FK_employee_departments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "dms",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employee_departments_departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "dms",
                        principalTable: "departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_departments_DepartmentHeadId",
                schema: "dms",
                table: "departments",
                column: "DepartmentHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_AppUserId",
                schema: "dms",
                table: "Clients",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_employee_departments_DepartmentId",
                schema: "dms",
                table: "employee_departments",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_departments_Employees_DepartmentHeadId",
                schema: "dms",
                table: "departments",
                column: "DepartmentHeadId",
                principalSchema: "dms",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_departments_Employees_DepartmentHeadId",
                schema: "dms",
                table: "departments");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "dms");

            migrationBuilder.DropTable(
                name: "employee_departments",
                schema: "dms");

            migrationBuilder.DropIndex(
                name: "IX_departments_DepartmentHeadId",
                schema: "dms",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "DepartmentHeadId",
                schema: "dms",
                table: "departments");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                schema: "dms",
                table: "Employees",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                schema: "dms",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                schema: "dms",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DepartmentId",
                schema: "dms",
                table: "AspNetUsers",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_departments_DepartmentId",
                schema: "dms",
                table: "AspNetUsers",
                column: "DepartmentId",
                principalSchema: "dms",
                principalTable: "departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_departments_DepartmentId",
                schema: "dms",
                table: "Employees",
                column: "DepartmentId",
                principalSchema: "dms",
                principalTable: "departments",
                principalColumn: "Id");
        }
    }
}
