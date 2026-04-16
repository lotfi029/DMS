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
                DefaultPermissions.Departments.Read
            ],
            [DefaultRoles.Employee] = [
                DefaultPermissions.Users.Read,
                DefaultPermissions.Departments.Read
            ]
        };
}