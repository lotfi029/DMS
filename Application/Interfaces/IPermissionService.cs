namespace Application.Interfaces;

public interface IPermissionService
{
    public Task<Result<IEnumerable<string>>> GetUserPermissionsAsync(string userId, CancellationToken ct = default);
    public Task<Result<IEnumerable<string>>> GetAllPermissionAsync(CancellationToken ct = default);
    public Task<Result> AssignPermissionToRoleAsync(string roleId, string permission, CancellationToken ct = default);
    public Task<Result> RemovePermissionFromRoleAsync(string roleId, string permission, CancellationToken ct = default);
    public Task<Result<IEnumerable<string>>> GetRolePermissionsAsync(string roleId, CancellationToken ct = default);
}
