using Application.DTOs.Permissions;

namespace Application.Features.Permissions.Queries.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string UserId) : IQuery<IEnumerable<PermissionResponse>>;

public sealed class GetUserPermissionsQueryHandler(IPermissionService service) : IQueryHandler<GetUserPermissionsQuery, IEnumerable<PermissionResponse>>
{
    public async Task<Result<IEnumerable<PermissionResponse>>> HandleAsync(GetUserPermissionsQuery query, CancellationToken ct = default)
        => await service.GetUserPermissionsAsync(query.UserId, ct);
}
