namespace Domain.Constants;

public static class DefaultPermissions
{
    public const string ClaimType = "permissions";

    public static class Users
    {
        public static string GroupName => nameof(Users);

        public const string Read = "users.read";
        public const string ViewProfile = "users.view_profile";
        public const string Update = "users.update";
        public const string Create = "users.create";
        public const string Deactivate = "users.deactivate";
        public const string Delete = "users.delete";

        public static IList<string> All =>
            typeof(Users).GetFields().Select(f => f.GetValue(f) as string).ToList()!;
    }
    public static class Roles
    {
        public static string GroupName => nameof(Roles);
        public const string Read = "roles.read";
        public const string Update = "roles.update";
        public const string Create = "roles.create";
        public const string AssignToUser = "roles.assign_to_user";
        public const string RemoveFromUser = "roles.remove_from_user";
        public const string Deactivate = "roles.deactivate";
        public const string Delete = "roles.delete";
        public static IList<string> All =>
            typeof(Roles).GetFields().Select(f => f.GetValue(f) as string).ToList()!;
    }
    public static class Departments
    {
        public static string GroupName => nameof(Departments);
        public const string Read = "departments.read";
        public const string ViewUsers = "departments.view_users";
        public const string Update = "departments.update";
        public const string Create = "departments.create";
        public const string Deactivate = "departments.deactivate";
        public const string Delete = "departments.delete";
        public const string AssignToUser = "departments.assign_to_user";
        public const string RemoveFromUser = "departments.remove_from_user";
        public const string MoveUser = "departments.move_user";
        public static IList<string> All =>
            typeof(Departments).GetFields().Select(f => f.GetValue(f) as string).ToList()!;
    }
    public static class Permissions
    {
        public static string GroupName => nameof(Permissions);

        public const string Read = "permissions.read";
        public const string AssignToRole = "permissions.assign_to_role";
        public const string RemoveFromRole = "permissions.remove_from_role";

        public static IList<string> All =>
            typeof(Permissions).GetFields().Select(f => f.GetValue(f) as string).ToList()!;
    }
    public static class Audit
    {
        public static string GroupName => nameof(Audit);
        public const string Read = "audit.read";
        public const string Export = "audit.export";
        public static IList<string> All =>
            typeof(Audit).GetFields().Select(f => f.GetValue(f) as string).ToList()!;
    }
    public static readonly IReadOnlyList<string> AllDefaultPermissions = [
        .. Users.All, 
        .. Roles.All, 
        .. Departments.All,
        .. Permissions.All,
        .. Audit.All];
}
