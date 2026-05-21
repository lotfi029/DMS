using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                schema: "dms",
                table: "Clients");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "dms",
                table: "user_permission_overrides",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dms",
                table: "user_permission_overrides",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dms",
                table: "Employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dms",
                table: "departments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dms",
                table: "Clients",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "dms",
                table: "audit_logs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "dms",
                table: "audit_logs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                schema: "dms",
                table: "audit_logs",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "dms",
                table: "user_permission_overrides");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dms",
                table: "user_permission_overrides");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dms",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dms",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dms",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "dms",
                table: "audit_logs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "dms",
                table: "audit_logs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                schema: "dms",
                table: "audit_logs");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                schema: "dms",
                table: "Clients",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
