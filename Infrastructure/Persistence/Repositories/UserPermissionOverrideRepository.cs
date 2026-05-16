namespace Infrastructure.Persistence.Repositories;

internal sealed class UserPermissionOverrideRepository(
    ApplicationDbContext dbContext) :
    GenericRepository<UserPermissionOverride>(dbContext),
    IUserPermissionOverrideRepository
{
    public async Task<IEnumerable<UserPermissionOverride>> GetActiveByUserIdAsync(
        string userId,
        CancellationToken ct = default)
    {
        return await DbContext.UserPermissionsOverride
            .Where(o =>
                o.UserId == userId &&
                (o.ExpiresAt == null || o.ExpiresAt > DateTime.UtcNow))
            .ToListAsync(ct);
    }

    public async Task<UserPermissionOverride?> GetByUserAndPermissionAsync(string userId, string permission, CancellationToken ct = default)
    {
        return await DbContext.UserPermissionsOverride
            .FirstOrDefaultAsync(
            o => o.UserId == userId && o.Permission == permission, ct);
    }
}