namespace Application.Features.Roles.Queries.GetUserRoles;

public sealed record GetUserRolesQuery(string UserId) : IQuery<IEnumerable<RoleResponse>>;

public sealed class GetUserRolesQueryHandler(
    IRoleService service,
    IAuditService auditService) : IQueryHandler<GetUserRolesQuery, IEnumerable<RoleResponse>>
{
    public async Task<Result<IEnumerable<RoleResponse>>> HandleAsync(GetUserRolesQuery query, CancellationToken ct = default)
    { 
        var result = await service.GetUserRolesAsync(query.UserId, ct);
        
        await auditService.LogActionAsync(
            action: AuditAction.RoleViewed,
            module: AuditModules.Roles,
            entityName: AuditEntityNames.Role,
            ct: ct);

        return result;
    }
}
