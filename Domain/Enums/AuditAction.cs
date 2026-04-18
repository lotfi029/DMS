namespace Domain.Enums;

public enum AuditAction
{
    Login = 1, Logout, TokenRefreshed, TokenRevoked,
    PasswordChanged, PasswordResetRequested,

    UserCreated = 100, UserUpdated, UserDeleted,
    UserActivated, UserDeactivated, UserViewed, UserListed,

    RoleCreated = 200, RoleUpdated, RoleDeleted,
    RoleAssignedToUser, RoleRemovedFromUser,
    RoleViewed,

    PermissionAssignedToRole = 300, PermissionRemovedFromRole, PermissionViewed,

    DepartmentCreated = 400, DepartmentUpdated, DepartmentDeleted,
    DepartmentViewed, DepartmentListed, DepartmentUserViewed,
    UserAddedToDepartment, UserRemovedFromDepartment, UserMovedBetweenDepartments,

    Read = 900, Create, Update, Delete, Export, Import,
}
