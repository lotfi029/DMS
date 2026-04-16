using Application.DTOs.Roles;

namespace Application.Features.Roles.Queries.GetUserRoles;

public sealed record GetUserRolesQuery(string UserId) : IQuery<IEnumerable<RoleResponse>>;

public sealed class GetUserRolesQueryHandler(IRoleService service) : IQueryHandler<GetUserRolesQuery, IEnumerable<RoleResponse>>
{
    public async Task<Result<IEnumerable<RoleResponse>>> HandleAsync(GetUserRolesQuery query, CancellationToken ct = default)
        => await service.GetUserRolesAsync(query.UserId, ct);
}
