namespace Domain.Enums;

public enum AuditAction
{
    Login = 1, Logout, TokenRefreshed, TokenRevoked,
    PasswordChanged, PasswordResetRequested,

    UserCreated = 100, UserUpdated, UserDeleted,
    UserActivated, UserDeactivated, UserViewed, UserListed,

    EmployeeCreated = 200, EmployeeUpdated, EmployeeDeleted,
    EmployeeActivated, EmployeeDeactivated, EmployeeViewed, EmployeeListed,

    RoleCreated = 300, RoleUpdated, RoleDeleted,
    RoleAssignedToUser, RoleRemovedFromUser,
    RoleViewed,

    PermissionAssignedToRole = 400, PermissionRemovedFromRole, PermissionViewed,
    PermissionGrantedToUser, PermissionDeniedFromUser, PermissionRevokedFromUser,

    DepartmentCreated = 500, DepartmentUpdated, DepartmentDeleted,
    DepartmentViewed, DepartmentListed, DepartmentUserViewed,
    UserAddedToDepartment, UserRemovedFromDepartment, UserMovedBetweenDepartments,
    AssignedDepartmentHead, RemovedDepartmentHead,

    ClientCreated = 600, ClientUpdated, ClientDeleted,
    ClientActivated, ClientDeactivated, ClientViewed, ClientListed,

    Read = 900, Create, Update, Delete, Export, Import,
}
