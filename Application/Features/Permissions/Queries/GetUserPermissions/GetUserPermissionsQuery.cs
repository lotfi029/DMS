using Application.DTOs.Permissions;

namespace Application.Features.Permissions.Queries.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string UserId) : IQuery<IEnumerable<PermissionResponse>>;

public sealed class GetUserPermissionsQueryHandler(
    IPermissionService service,
    IAuditService auditService) : IQueryHandler<GetUserPermissionsQuery, IEnumerable<PermissionResponse>>
{
    public async Task<Result<IEnumerable<PermissionResponse>>> HandleAsync(GetUserPermissionsQuery query, CancellationToken ct = default)
    { 
        var result = await service.GetUserPermissionsAsync(query.UserId, ct);

        await auditService.LogActionAsync(
            action: AuditAction.PermissionViewed,
            module: AuditModules.Permissions,
            entityName: AuditEntityNames.RoleClaim,
            ct: ct);

        return result;
    
    }
}
