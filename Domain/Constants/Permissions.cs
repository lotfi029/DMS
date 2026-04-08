namespace Domain.Constants;

public static class Permissions
{
    public const string ClaimType = "permission";

    public static class Users
    {
        public const string Read = "users.read";
        public const string Update = "users.update";
        public const string Create = "users.create";
        public const string Deactivate = "users.deactivate";
        public const string Delete = "users.delete";

        public static IList<string> AllUsersPermissions =>
            typeof(Users).GetFields().Select(f => f.GetValue(f) as string).ToList()!;
    }
    public static class Roles
    {
        public const string Read = "roles.read";
        public const string Update = "roles.update";
        public const string Create = "roles.create";
        public const string Deactivate = "roles.deactivate";
        public const string Delete = "roles.delete";
        public static IList<string> AllRolesPermissions =>
            typeof(Roles).GetFields().Select(f => f.GetValue(f) as string).ToList()!;
    }
    public static class Departments
    {
        public const string Read = "departments.read";
        public const string Update = "departments.update";
        public const string Create = "departments.create";
        public const string Deactivate = "departments.deactivate";
        public const string Delete = "departments.delete";
        public static IList<string> AllDepartmentsPermissions =>
            typeof(Departments).GetFields().Select(f => f.GetValue(f) as string).ToList()!;
    }

    public static readonly IReadOnlyList<string> AllPermissions = [
        .. Users.AllUsersPermissions, 
        .. Roles.AllRolesPermissions, 
        .. Departments.AllDepartmentsPermissions];
}
