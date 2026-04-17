using Application.DTOs.Permissions;

namespace Application.Features.Permissions.Queries.GetAll;

public sealed record GetAllPermissionsQuery() : IQuery<IEnumerable<PermissionResponse>>;

public sealed class GetAllPermissionsQueryHandler(IPermissionService service) : IQueryHandler<GetAllPermissionsQuery, IEnumerable<PermissionResponse>>
{
    public async Task<Result<IEnumerable<PermissionResponse>>> HandleAsync(GetAllPermissionsQuery query, CancellationToken ct = default)
        => await service.GetAllPermissionAsync(ct);
}
