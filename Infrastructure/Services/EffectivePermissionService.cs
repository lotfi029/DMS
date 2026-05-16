namespace Infrastructure.Services;

internal sealed class EffectivePermissionService(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : IEffectivePermissionService
{
    public async Task<IEnumerable<string>> ResolveAsync(string userId, CancellationToken ct = default)
    {
        if (await userManager.FindByIdAsync(userId) is not { } user)
            return [];

        var roles = await userManager.GetRolesAsync(user);

        var rolePermissions = await (
            from r in context.Roles
            join rc in context.RoleClaims on r.Id equals rc.RoleId
            where roles.Contains(r.Name!) && rc.ClaimType == DefaultPermissions.ClaimType
            select rc.ClaimValue).Distinct().ToListAsync(ct);

        var overrides = await context.UserPermissionsOverride
            .AsNoTracking()
            .Where(o =>
                o.UserId == userId &&
                (o.ExpiresAt == null || o.ExpiresAt > DateTime.UtcNow))
            .ToListAsync(ct);

        var effective = new HashSet<string>(rolePermissions);

        foreach(var o in overrides)
        {
            if (o.IsGranted)
                effective.Add(o.Permission);
            else
                effective.Remove(o.Permission);
        }
        
        return effective;
    }
}