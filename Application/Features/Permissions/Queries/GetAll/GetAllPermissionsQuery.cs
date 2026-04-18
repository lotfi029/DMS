namespace Application.Features.Permissions.Queries.GetAll;

public sealed record GetAllPermissionsQuery() : IQuery<IEnumerable<PermissionResponse>>;

public sealed class GetAllPermissionsQueryHandler(
    IPermissionService service,
    IAuditService auditService) : IQueryHandler<GetAllPermissionsQuery, IEnumerable<PermissionResponse>>
{
    public async Task<Result<IEnumerable<PermissionResponse>>> HandleAsync(GetAllPermissionsQuery query, CancellationToken ct = default)
    {
        var result = await service.GetAllPermissionAsync(ct);

        await auditService.LogActionAsync(
            action: AuditAction.PermissionViewed,
            module: AuditModules.Permissions,
            entityName: AuditEntityNames.RoleClaim,
            ct: ct);

        return result;
    }
}
