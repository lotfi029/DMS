using Application.DTOs.Audits;

namespace Infrastructure.Services;

internal sealed class AuditService(
    ApplicationDbContext dbContext,
    AuditContextAccessor contextAccessor,
    ILogger<AuditService> logger) : IAuditService
{
    // ── Write ─────────────────────────────────────────────────────────────────

    public async Task LogAsync(AuditLog entry, CancellationToken ct = default)
    {
        try
        {
            dbContext.AuditLogs.Add(entry);
            await dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            // Never let audit failure crash the application
            logger.LogError(ex, "Failed to persist audit log entry. Action={Action} Entity={Entity}",
                entry.Action, entry.EntityName);
        }
    }

    public async Task LogActionAsync(
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
        CancellationToken ct = default)
    {
        var ctx = contextAccessor.GetCurrent();

        var entry = AuditLog.Create(
            action: action,
            entityName: entityName,
            module: module,
            description: description,
            entityId: entityId,
            userId: ctx.UserId,
            userName: ctx.UserName,
            userEmail: ctx.UserEmail,
            ipAddress: ctx.IpAddress,
            userAgent: ctx.UserAgent,
            requestPath: ctx.RequestPath,
            requestMethod: ctx.RequestMethod,
            oldValues: oldValues,
            newValues: newValues,
            changedColumns: changedColumns,
            outcome: outcome,
            failureReason: failureReason
        );

        await LogAsync(entry, ct);
    }

    // ── Read ──────────────────────────────────────────────────────────────────

    public async Task<AuditPagedResult> GetLogsAsync(
        AuditLogQuery query, CancellationToken ct = default)
    {
        var q = dbContext.AuditLogs.AsNoTracking();

        // Filters
        if (!string.IsNullOrWhiteSpace(query.UserId))
            q = q.Where(x => x.UserId == query.UserId);

        if (!string.IsNullOrWhiteSpace(query.Module))
            q = q.Where(x => x.Module == query.Module);

        if (query.Action.HasValue)
            q = q.Where(x => x.Action == query.Action.Value);

        if (query.Outcome.HasValue)
            q = q.Where(x => x.Outcome == query.Outcome.Value);

        if (!string.IsNullOrWhiteSpace(query.EntityName))
            q = q.Where(x => x.EntityName == query.EntityName);

        if (!string.IsNullOrWhiteSpace(query.EntityId))
            q = q.Where(x => x.EntityId == query.EntityId);

        if (query.From.HasValue)
            q = q.Where(x => x.Timestamp >= query.From.Value);

        if (query.To.HasValue)
            q = q.Where(x => x.Timestamp <= query.To.Value);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.ToLower();
            q = q.Where(x =>
                (x.Description != null && x.Description.ToLower().Contains(term)) ||
                (x.UserName != null && x.UserName.ToLower().Contains(term)) ||
                (x.EntityId != null && x.EntityId.ToLower().Contains(term)) ||
                (x.UserEmail != null && x.UserEmail.ToLower().Contains(term)));
        }

        var totalCount = await q.CountAsync(ct);

        // Sorting
        q = (query.SortBy?.ToLower(), query.Descending) switch
        {
            ("action", true) => q.OrderByDescending(x => x.Action),
            ("action", false) => q.OrderBy(x => x.Action),
            ("username", true) => q.OrderByDescending(x => x.UserName),
            ("username", false) => q.OrderBy(x => x.UserName),
            ("module", true) => q.OrderByDescending(x => x.Module),
            ("module", false) => q.OrderBy(x => x.Module),
            (_, true) => q.OrderByDescending(x => x.Timestamp),
            _ => q.OrderBy(x => x.Timestamp)
        };

        var items = await q
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ProjectToType<AuditLogResponse>()
            .ToListAsync(ct);

        return new AuditPagedResult(
            Items: items,
            TotalCount: totalCount,
            Page: query.Page,
            PageSize: query.PageSize,
            TotalPages: (int)Math.Ceiling((double)totalCount / query.PageSize));
    }

    public async Task<IEnumerable<AuditLog>> GetEntityHistoryAsync(
        string entityName, string entityId, CancellationToken ct = default) =>
        await dbContext.AuditLogs
            .AsNoTracking()
            .Where(x => x.EntityName == entityName && x.EntityId == entityId)
            .OrderByDescending(x => x.Timestamp)
            .Take(200)
            .ToListAsync(ct);

    public async Task<IEnumerable<AuditLog>> GetUserActivityAsync(
        string userId, int pageSize = 50, CancellationToken ct = default) =>
        await dbContext.AuditLogs
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Timestamp)
            .Take(pageSize)
            .ToListAsync(ct);
}