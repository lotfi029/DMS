namespace Application.Features.Permissions.Queries.GetUserPermissions;

public sealed record GetUserPermissionsQuery(string UserId) : IQuery<IEnumerable<string>>;

public sealed class GetUserPermissionsQueryHandler(IPermissionService service) : IQueryHandler<GetUserPermissionsQuery, IEnumerable<string>>
{
    public async Task<Result<IEnumerable<string>>> HandleAsync(GetUserPermissionsQuery query, CancellationToken ct = default)
        => await service.GetUserPermissionsAsync(query.UserId, ct);
}
