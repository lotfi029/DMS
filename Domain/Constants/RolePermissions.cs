namespace Domain.Constants;

public static class RolePermissions
{
    public static readonly IReadOnlyDictionary<ApplicationRole, IReadOnlyList<string>> DefaultRolePermissions
        = new Dictionary<ApplicationRole, IReadOnlyList<string>>()
        {
            [DefaultRoles.Manager] = [.. DefaultPermissions.AllDefaultPermissions],
            [DefaultRoles.Admin] = [.. DefaultPermissions.AllDefaultPermissions],
            [DefaultRoles.DepartmentHead] = [
                DefaultPermissions.Users.Read,
                DefaultPermissions.Employees.Read,
                DefaultPermissions.Employees.ViewDetails,
                DefaultPermissions.Departments.Read,
                DefaultPermissions.Departments.ViewUsers,
                DefaultPermissions.Departments.AssignToUser,
                DefaultPermissions.Roles.Read,
                DefaultPermissions.Roles.AssignToUser,
                DefaultPermissions.Permissions.Read,
                DefaultPermissions.Permissions.Grant,
            ],
            [DefaultRoles.Employee] = [
                DefaultPermissions.Users.Read,
                DefaultPermissions.Users.ViewProfile,
                DefaultPermissions.Departments.Read
            ],
        };
}