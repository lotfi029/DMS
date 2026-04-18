namespace Application.Features.Roles.Queries.GetAll;

public sealed record GetAllRolesQuery : IQuery<IEnumerable<RoleResponse>>;

public sealed class GetAllRolesQueryHandler(
    IRoleService service,
    IAuditService auditService) : IQueryHandler<GetAllRolesQuery, IEnumerable<RoleResponse>>
{
    public async Task<Result<IEnumerable<RoleResponse>>> HandleAsync(GetAllRolesQuery query, CancellationToken ct = default)
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
