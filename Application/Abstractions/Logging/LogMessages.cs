namespace Application.Abstractions.Logging;

/// <summary>
/// Centralized structured log message templates.
/// Use these with ILogger — Serilog reads them as structured messages.
///
/// Pattern: {Subject}_{Verb}  →  e.g. User_Created, Role_Deleted
///
/// Usage:
///   _logger.LogInformation(LogMessages.Command_Handling, typeof(TCommand).Name);
///   _logger.LogError(LogMessages.Command_Failed, typeof(TCommand).Name, result.Error.Description);
/// </summary>
public static class LogMessages
{
    // ── CQRS Pipeline ─────────────────────────────────────────────────────────
    public const string Command_Handling = "Handling command {CommandName}";
    public const string Command_Succeeded = "Command {CommandName} completed successfully";
    public const string Command_Failed = "Command {CommandName} failed. Error: {ErrorDescription}";
    public const string Command_Exception = "Unhandled exception in command {CommandName}";

    public const string Query_Handling = "Handling query {QueryName}";
    public const string Query_Succeeded = "Query {QueryName} completed successfully";
    public const string Query_Failed = "Query {QueryName} failed. Error: {ErrorDescription}";
    public const string Query_Exception = "Unhandled exception in query {QueryName}";

    // ── Auth ──────────────────────────────────────────────────────────────────
    public const string Auth_LoginAttempt = "Login attempt for {Email}";
    public const string Auth_LoginSuccess = "User {UserName} logged in successfully from {IpAddress}";
    public const string Auth_LoginFailed = "Login failed for {Email}. Reason: {Reason}";
    public const string Auth_TokenRefreshed = "Token refreshed for user {UserId}";
    public const string Auth_TokenRevoked = "Refresh token revoked for user {UserId}";
    public const string Auth_Logout = "User {UserName} logged out";

    // ── Users ─────────────────────────────────────────────────────────────────
    public const string User_Creating = "Creating user {UserName} with email {Email}";
    public const string User_Created = "User {UserId} created successfully. UserName: {UserName}";
    public const string User_CreateFailed = "Failed to create user {UserName}. Error: {ErrorDescription}";
    public const string User_Updated = "User {UserId} profile updated";
    public const string User_UpdateFailed = "Failed to update user {UserId}. Error: {ErrorDescription}";
    public const string User_Activated = "User {UserId} activated";
    public const string User_Deactivated = "User {UserId} deactivated";
    public const string User_Deleted = "User {UserId} soft-deleted";
    public const string User_NotFound = "User {UserId} not found";
    public const string User_FetchAll = "Fetching all users excluding {RequestingUserId}";

    // ── Roles ─────────────────────────────────────────────────────────────────
    public const string Role_Creating = "Creating role {RoleName}";
    public const string Role_Created = "Role {RoleName} created successfully";
    public const string Role_CreateFailed = "Failed to create role {RoleName}. Error: {ErrorDescription}";
    public const string Role_Updated = "Role {RoleId} renamed to {NewRoleName}";
    public const string Role_UpdateFailed = "Failed to update role {RoleId}. Error: {ErrorDescription}";
    public const string Role_Deleted = "Role {RoleId} deleted";
    public const string Role_DeleteFailed = "Failed to delete role {RoleId}. Error: {ErrorDescription}";
    public const string Role_AssignedToUser = "Role {RoleId} assigned to user {UserId}";
    public const string Role_RemovedFromUser = "Role {RoleId} removed from user {UserId}";
    public const string Role_NotFound = "Role {RoleId} not found";

    // ── Permissions ───────────────────────────────────────────────────────────
    public const string Permission_Assigned = "Permission {Permission} assigned to role {RoleId}";
    public const string Permission_Removed = "Permission {Permission} removed from role {RoleId}";
    public const string Permission_AlreadyAssigned = "Permission {Permission} already assigned to role {RoleId}";

    // ── Departments ───────────────────────────────────────────────────────────
    public const string Dept_Creating = "Creating department {DepartmentName}";
    public const string Dept_Created = "Department {DepartmentName} created successfully";
    public const string Dept_CreateFailed = "Failed to create department {DepartmentName}. Error: {ErrorDescription}";
    public const string Dept_Updated = "Department {DepartmentId} updated";
    public const string Dept_Deleted = "Department {DepartmentId} deleted";
    public const string Dept_NotFound = "Department {DepartmentId} not found";
    public const string Dept_UserAdded = "User {UserId} added to department {DepartmentId}";
    public const string Dept_UserRemoved = "User {UserId} removed from department {DepartmentId}";
    public const string Dept_UserMoved = "User {UserId} moved to department {DepartmentId}";

    // ── MOMS Projects ─────────────────────────────────────────────────────────
    public const string Project_Created = "Project {ProjectCode} created for client {ClientName}";
    public const string Project_Updated = "Project {ProjectId} updated";
    public const string Project_StageAdvanced = "Project {ProjectId} advanced to stage {Stage}";
    public const string Project_GateApproved = "Stage gate {Stage} approved for project {ProjectId} by {ApprovedBy}";
    public const string Project_GateBlocked = "Stage gate {Stage} blocked for project {ProjectId}. Reason: {Reason}";

    // ── Infrastructure ────────────────────────────────────────────────────────
    public const string DB_MigrationApplied = "Database migrations applied successfully";
    public const string DB_SeedStarted = "Database seeding started for {SeederName}";
    public const string DB_SeedCompleted = "Database seeding completed for {SeederName}";
    public const string DB_SeedSkipped = "{EntityName} '{EntityValue}' already exists — skipping seed";

    public const string Audit_LogFailed = "Failed to persist audit log. Action={Action} Entity={EntityName}. Exception: {ExceptionMessage}";
    public const string Audit_Interceptor = "Audit interceptor captured {Count} entries for save operation";
}