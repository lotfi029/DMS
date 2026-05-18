namespace Application.Features.Permissions.Queries.GetEffective;

public sealed record GetUserEffectivePermissionsQuery(
    string UserId
    ) : IQuery<EffectivePermissionsResponse>;
public sealed record EffectivePermissionsResponse(
    string UserId,
    IEnumerable<string> Permissions,
    IEnumerable<PermissionOverrideDto> Overrides
);
public sealed record PermissionOverrideDto(
    Guid Id,
    string Permission,
    bool IsGranted,
    string GrantedById,
    string? Reason,
    DateTime CreatedAt,
    DateTime? ExpiresAt
    );


internal sealed class GetUserEffectivePermissionsQueryHandler(
    IEffectivePermissionService permissionService,
    IUserPermissionOverrideRepository repo
    ) : IQueryHandler<GetUserEffectivePermissionsQuery, EffectivePermissionsResponse>
{
    public async Task<Result<EffectivePermissionsResponse>> HandleAsync(GetUserEffectivePermissionsQuery query, CancellationToken ct = default)
    {
        var effectivePermissions = await permissionService.ResolveAsync(query.UserId, ct);
        var overrides = await repo.GetActiveByUserIdAsync(query.UserId, ct);

        return new EffectivePermissionsResponse(
            query.UserId,
            effectivePermissions,
            overrides.Select(o => new PermissionOverrideDto(
                o.Id,
                o.Permission,
                o.IsGranted,
                o.GrantedById,
                o.Reason,
                o.CreatedAt,
                o.ExpiresAt
            )));
    }
}