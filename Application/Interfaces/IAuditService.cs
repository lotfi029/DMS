using Application.DTOs.Audits;

namespace Application.Interfaces;

public interface IAuditService
{
    Task LogAsync(AuditLog log, CancellationToken ct = default);


    Task LogActionAsync(
        AuditAction action,
        string module,
        string entityName,
        string? entityId = null,
        string? description = null,
        string? oldValues = null,
        string? newValues = null,
        string? changedColumns = null,
        AuditOutcome outcome = AuditOutcome.Success,
        string? failureReason = null,
        CancellationToken ct = default
        );

    Task<AuditPagedResult> GetLogsAsync(AuditLogQuery query, CancellationToken ct = default);
    Task<IEnumerable<AuditLog>> GetEntityHistoryAsync(string entityName, string entityId, CancellationToken ct = default);
    Task<IEnumerable<AuditLog>> GetUserActivityAsync(string userId, int pageSize = 50, CancellationToken ct = default);
}
