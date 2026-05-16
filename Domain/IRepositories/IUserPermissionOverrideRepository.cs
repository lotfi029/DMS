namespace Domain.IRepositories;

public interface IUserPermissionOverrideRepository : IGenericRepository<UserPermissionOverride>
{
    Task<IEnumerable<UserPermissionOverride>> GetActiveByUserIdAsync(
        string userId, CancellationToken ct = default);

    Task<UserPermissionOverride?> GetByUserAndPermissionAsync(
        string userId, string permission, CancellationToken ct = default);
}