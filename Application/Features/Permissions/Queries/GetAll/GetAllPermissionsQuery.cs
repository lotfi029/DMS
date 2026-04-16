namespace Application.Features.Permissions.Queries.GetAll;

public sealed record GetAllPermissionsQuery() : IQuery<IEnumerable<string>>;

public sealed class GetAllPermissionsQueryHandler(IPermissionService service) : IQueryHandler<GetAllPermissionsQuery, IEnumerable<string>>
{
    public async Task<Result<IEnumerable<string>>> HandleAsync(GetAllPermissionsQuery query, CancellationToken ct = default)
        => await service.GetAllPermissionAsync(ct);
}
