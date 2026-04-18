namespace Application.DTOs.Audits;

public sealed record AuditLogQuery(
    string? UserId = null,
    string? Module = null,
    AuditAction? Action = null,
    AuditOutcome? Outcome = null,
    string? EntityName = null,
    string? EntityId = null,
    DateTime? From = null,
    DateTime? To = null,
    string? SearchTerm = null,   // searched in Description, UserName, EntityId
    int Page = 1,
    int PageSize = 25,
    string SortBy = "Timestamp",
    bool Descending = true);

public sealed record AuditPagedResult(
    IEnumerable<AuditLogResponse> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages);

public sealed record AuditLogResponse(
    Guid Id,
    string? UserId,
    string? UserName,
    string? UserEmail,
    string? IpAddress,
    string Action,
    string EntityName,
    string? EntityId,
    string? Module,
    string? Description,
    string Outcome,
    string? FailureReason,
    string? OldValues,
    string? NewValues,
    string? ChangedColumns,
    string? RequestPath,
    string? RequestMethod,
    long? DurationMs,
    DateTime Timestamp);

public sealed record AuditContext(
    string? UserId,
    string? UserName,
    string? UserEmail,
    string? IpAddress,
    string? UserAgent,
    string? RequestPath,
    string? RequestMethod);