using Application.DTOs.Permissions;

namespace Application.Features.Permissions.Queries.GetRolePermissions;

public sealed record GetRolePermissionsQuery(string RoleId) : IQuery<IEnumerable<PermissionResponse>>;

public sealed class GetRolePermissionsQueryHandler(IPermissionService service) : IQueryHandler<GetRolePermissionsQuery, IEnumerable<PermissionResponse>>
{
    public async Task<Result<IEnumerable<PermissionResponse>>> HandleAsync(GetRolePermissionsQuery query, CancellationToken ct = default)
        => await service.GetRolePermissionsAsync(query.RoleId, ct);
}
