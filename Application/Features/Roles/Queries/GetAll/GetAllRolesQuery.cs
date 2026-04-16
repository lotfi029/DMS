using Application.DTOs.Roles;

namespace Application.Features.Roles.Queries.GetAll;

public sealed record GetAllRolesQuery() : IQuery<IEnumerable<RoleResponse>>;

public sealed class GetAllRolesQueryHandler(IRoleService service) : IQueryHandler<GetAllRolesQuery, IEnumerable<RoleResponse>>
{
    public async Task<Result<IEnumerable<RoleResponse>>> HandleAsync(GetAllRolesQuery query, CancellationToken ct = default)
        => await service.GetAllRolesAsync(ct);
}
