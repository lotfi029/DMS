namespace Application.Features.Roles.Queries.GetAll;

public sealed record GetAllRolesQuery : IQuery<IEnumerable<RoleListResponse>>;

public sealed class GetAllRolesQueryHandler(
    IRoleService service,
    IAuditService auditService) : IQueryHandler<GetAllRolesQuery, IEnumerable<RoleListResponse>>
{
    public async Task<Result<IEnumerable<RoleListResponse>>> HandleAsync(GetAllRolesQuery query, CancellationToken ct = default)
    {
        var result = await service.GetAllRolesAsync(ct);

        await auditService.LogActionAsync(
            action: AuditAction.RoleViewed,
            module: AuditModules.Roles,
            entityName: AuditEntityNames.Role,
            outcome: AuditOutcome.Success,
            ct: ct);

        return result;
    }
}
