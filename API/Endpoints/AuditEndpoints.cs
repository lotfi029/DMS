using Application.DTOs.Audits;

namespace API.Endpoints;

internal sealed class AuditEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/audit")
            .WithTags("Audit")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        // Full paginated audit log — Admin only
        group.MapGet("/", GetLogsAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Audit.Read))
            .Produces<AuditPagedResult>(StatusCodes.Status200OK);

        // Entity change history
        group.MapGet("/entity/{entityName}/{entityId}", GetEntityHistoryAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Audit.Read))
            .Produces<IEnumerable<AuditLogResponse>>(StatusCodes.Status200OK);

        // Current user's own activity (any role — self-service)
        group.MapGet("/my-activity", GetMyActivityAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Users.ViewProfile))
            .Produces<IEnumerable<AuditLogResponse>>(StatusCodes.Status200OK);

        // Specific user's activity — Admin only
        group.MapGet("/user/{userId}", GetUserActivityAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Audit.Read))
            .Produces<IEnumerable<AuditLogResponse>>(StatusCodes.Status200OK);
    }

    private static async Task<IResult> GetLogsAsync(
        [AsParameters] AuditLogQueryParams p,
        [FromServices] IAuditService auditService,
        CancellationToken ct)
    {
        var query = new AuditLogQuery(
            UserId: p.UserId, Module: p.Module,
            Action: p.Action, Outcome: p.Outcome,
            EntityName: p.EntityName, EntityId: p.EntityId,
            From: p.From, To: p.To,
            SearchTerm: p.Search,
            Page: p.Page, PageSize: Math.Min(p.PageSize, 100),
            SortBy: p.SortBy ?? "Timestamp", Descending: p.Desc);

        var result = await auditService.GetLogsAsync(query, ct);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetEntityHistoryAsync(
        string entityName, string entityId,
        [FromServices] IAuditService auditService,
        CancellationToken ct)
    {
        var logs = await auditService.GetEntityHistoryAsync(entityName, entityId, ct);
        return Results.Ok(logs.Adapt<IEnumerable<AuditLogResponse>>());
    }

    private static async Task<IResult> GetMyActivityAsync(
        HttpContext ctx,
        [FromServices] IAuditService auditService,
        CancellationToken ct)
    {
        var userId = ctx.GetUserId();
        var logs = await auditService.GetUserActivityAsync(userId, 100, ct);
        return Results.Ok(logs.Adapt<IEnumerable<AuditLogResponse>>());
    }

    private static async Task<IResult> GetUserActivityAsync(
        string userId,
        [FromServices] IAuditService auditService,
        CancellationToken ct)
    {
        var logs = await auditService.GetUserActivityAsync(userId, 200, ct);
        return Results.Ok(logs.Adapt<IEnumerable<AuditLogResponse>>());
    }
}
public record AuditLogQueryParams(
    [FromQuery] string? UserId = null,
    [FromQuery] string? Module = null,
    [FromQuery] AuditAction? Action = null,
    [FromQuery] AuditOutcome? Outcome = null,
    [FromQuery] string? EntityName = null,
    [FromQuery] string? EntityId = null,
    [FromQuery] DateTime? From = null,
    [FromQuery] DateTime? To = null,
    [FromQuery] string? Search = null,
    [FromQuery] int Page = 1,
    [FromQuery] int PageSize = 25,
    [FromQuery] string? SortBy = "Timestamp",
    [FromQuery] bool Desc = true);