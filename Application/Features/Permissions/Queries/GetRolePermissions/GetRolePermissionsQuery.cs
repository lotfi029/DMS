namespace Application.Features.Permissions.Queries.GetRolePermissions;

public sealed record GetRolePermissionsQuery(string RoleId) : IQuery<IEnumerable<string>>;

public sealed class GetRolePermissionsQueryHandler(IPermissionService service) : IQueryHandler<GetRolePermissionsQuery, IEnumerable<string>>
{
    public async Task<Result<IEnumerable<string>>> HandleAsync(GetRolePermissionsQuery query, CancellationToken ct = default)
        => await service.GetRolePermissionsAsync(query.RoleId, ct);
}
