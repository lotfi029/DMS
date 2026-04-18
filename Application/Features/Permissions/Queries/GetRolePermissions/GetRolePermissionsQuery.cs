namespace Application.Features.Permissions.Queries.GetRolePermissions;

public sealed record GetRolePermissionsQuery(string RoleId) : IQuery<IEnumerable<PermissionResponse>>;

public sealed class GetRolePermissionsQueryHandler(
    IPermissionService service,
    IAuditService auditService) : IQueryHandler<GetRolePermissionsQuery, IEnumerable<PermissionResponse>>
{
    public async Task<Result<IEnumerable<PermissionResponse>>> HandleAsync(GetRolePermissionsQuery query, CancellationToken ct = default)
    {
        var result = await service.GetRolePermissionsAsync(query.RoleId, ct);

        await auditService.LogActionAsync(
            action: AuditAction.PermissionViewed,
            module: AuditModules.Permissions,
            entityName: AuditEntityNames.RoleClaim,
            ct: ct
            );

        return result;
    }
}
