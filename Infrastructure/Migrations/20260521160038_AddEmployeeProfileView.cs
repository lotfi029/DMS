using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeProfileView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE OR REPLACE VIEW dms."V_EmployeeProfile" AS
                SELECT
                    e."Id"                                      AS "EmployeeId",
                    e."AppUserId"                               AS "UserId",
                    u."FirstName",
                    u."LastName",
                    u."FirstName" || ' ' || u."LastName"        AS "FullName",
                    u."Email",
                    u."UserName",
                    u."PhoneNumber"                             AS "UserPhone",
                    u."IsActive"                                AS "UserIsActive",
                    u."LastLoginAt",
                    u."CreatedAt",
                    e."JobTitle",
                    e."ContractType",
                    e."PhoneNumber"                             AS "EmployeePhone",
                    e."EmergencyContactName",
                    e."EmergencyContactPhone",
                    e."HireDate",
                    e."EndDate",
                    e."IsActive"                                AS "EmployeeIsActive",
                    e."Notes",
                    e."CreatedAt"                               AS "EmployeeCreatedAt",
                    d."Id"                                      AS "DepartmentId",
                    d.name                                    AS "DepartmentName",
                    ur."RoleId"                                 AS "UserRoleId",
                    r."Name"                                    AS "RoleName",
                    dh."Id"                                     AS "DeptHeadId",
                    dhu."FirstName" || ' ' || dhu."LastName"    AS "DeptHeadName"
                FROM dms."Employees" e
                JOIN     dms."AspNetUsers"         u   ON u."Id"           = e."AppUserId"
                LEFT JOIN dms."employee_departments" ed ON ed."EmployeeId" = e."Id"
                                                       AND ed."IsActive"   = true
                LEFT JOIN dms."AspNetUserRoles"      ur ON ur."UserId"     = e."AppUserId"
                LEFT JOIN dms."AspNetRoles"                r  ON r."Id"          = ur."RoleId"
                LEFT JOIN dms."departments"          d  ON d."Id"          = ed."DepartmentId"
                LEFT JOIN dms."Employees"            dh ON dh."Id"         = d."DepartmentHeadId"
                LEFT JOIN dms."AspNetUsers"         dhu ON dhu."Id"        = dh."AppUserId";
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP VIEW IF EXISTS dms."V_EmployeeProfile";
            """);
        }
    }
}
