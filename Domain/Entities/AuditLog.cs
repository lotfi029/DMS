namespace Domain.Entities;

public sealed class AuditLog : Entity
{
    public string? UserId { get; private set; }
    public string? UserName { get; private set; }
    public string? UserEmail { get; private set; }
    public string? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }

    public AuditAction Action { get; private set; }
    public string EntityName { get; private set; } = string.Empty;
    public string? EntityId { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }
    public string? ChangedColumns { get; private set; }

    public string? Module { get; private set; }
    public string? Description { get; private set; }
    public AuditOutcome Outcome { get; private set; }
    public string? FailureReason { get; private set; }

    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;
    public string? RequestPath { get; private set; }
    public string? RequestMethod { get; private set; }
    public long? DurationMs { get; private set; }

    private AuditLog() : base() { }

    public static AuditLog Create(
        AuditAction action,
        string entityName,
        string module,
        string? description = null,
        string? entityId = null,
        string? userId = null,
        string? userName = null,
        string? userEmail = null,
        string? ipAddress = null,
        string? userAgent = null,
        string? requestPath = null,
        string? requestMethod = null,
        string? oldValues = null,
        string? newValues = null,
        string? changedColumns = null,
        AuditOutcome outcome = AuditOutcome.Success,
        string? failureReason = null,
        long? durationMs = null) => new()
        {
            Action = action,
            EntityName = entityName,
            Module = module,
            Description = description,
            EntityId = entityId,
            UserId = userId,
            UserName = userName,
            UserEmail = userEmail,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            RequestPath = requestPath,
            RequestMethod = requestMethod,
            OldValues = oldValues,
            NewValues = newValues,
            ChangedColumns = changedColumns,
            Outcome = outcome,
            FailureReason = failureReason,
            DurationMs = durationMs,
            Timestamp = DateTime.UtcNow
        };

    public static AuditLog Login(
        string userId, string userName, string email,
        string? ip, bool success, string? resone = null) =>
        Create(
            action: AuditAction.Login, entityName: nameof(ApplicationUser), "Auth",
            description: success
            ? $"User {userName} logged in successfully."
            : $"Failed login attempt for user {email}.",
            entityId: userId, userId: userId, userName: userName, userEmail: email, ipAddress: ip,
            outcome: success ? AuditOutcome.Success : AuditOutcome.Failure,
            failureReason: resone
            );

    public static AuditLog Logout(
        string userId, string userName,
        string? ip) =>
        Create(
            action: AuditAction.Logout, entityName: nameof(ApplicationUser), "Auth",
            description: $"User {userName} logged out.",
            entityId: userId, userId: userId, userName: userName, ipAddress: ip,
            outcome: AuditOutcome.Success
            );

    public static AuditLog TokenRefreshed(string userId, string? ip) =>
        Create(
            action: AuditAction.TokenRefreshed, entityName: nameof(ApplicationUser), "Auth",
            description: $"User {userId} refreshed authentication token.",
            entityId: userId, userId: userId, ipAddress: ip,
            outcome: AuditOutcome.Success
            );

    public static AuditLog TokenRevoked(string userId, string? ip) =>
        Create(
            action: AuditAction.TokenRevoked, entityName: nameof(ApplicationUser), "Auth",
            description: $"User {userId} revoked authentication token.",
            entityId: userId, userId: userId, ipAddress: ip,
            outcome: AuditOutcome.Success
            );

}


public enum AuditAction
{
    Login = 1, Logout, TokenRefreshed, TokenRevoked,
    PasswordChanged, PasswordResetRequested,

    UserCreated = 100, UserUpdated, UserDeleted,
    UserActivated, UserDeactivated, UserViewed, UserListed,

    RoleCreated = 200, RoleUpdated, RoleDeleted,
    RoleAssignedToUser, RoleRemovedFromUser,

    PermissionAssignedToRole = 300, PermissionRemovedFromRole, PermissionViewed,

    DepartmentCreated = 400, DepartmentUpdated, DepartmentDeleted,
    DepartmentViewed, DepartmentListed,
    UserAddedToDepartment, UserRemovedFromDepartment, UserMovedBetweenDepartments,

    Read = 900, Create, Update, Delete, Export, Import,
}
public enum AuditOutcome
{
    Success = 1, Failure, Unauthorized, NotFound, ValidationError
}