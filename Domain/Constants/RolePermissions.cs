using Domain.Entities;

namespace Domain.Constants;

public static class RolePermissions
{
    public static readonly IReadOnlyDictionary<ApplicationRole, IReadOnlyList<string>> DefaultRolePermissions
        = new Dictionary<ApplicationRole, IReadOnlyList<string>>()
        {
            [DefaultRoles.Manager] = [.. Permissions.AllPermissions],
            [DefaultRoles.Admin] = [.. Permissions.AllPermissions],
            [DefaultRoles.DepartmentHead] = [
                Permissions.Users.Read, 
                Permissions.Departments.Read
            ],
            [DefaultRoles.Engineer] = [
                Permissions.Users.Read,
                Permissions.Departments.Read
            ]
        };
}