using Application.DTOs.Permissions;

namespace Application.Interfaces;

public interface IPermissionService
{
    public Task<Result<IEnumerable<PermissionResponse>>> GetUserPermissionsAsync(string userId, CancellationToken ct = default);
    public Task<Result<IEnumerable<PermissionResponse>>> GetAllPermissionAsync(CancellationToken ct = default);
    public Task<Result> AssignPermissionToRoleAsync(string roleId, string permission, CancellationToken ct = default);
    public Task<Result> RemovePermissionFromRoleAsync(string roleId, string permission, CancellationToken ct = default);
    public Task<Result<IEnumerable<PermissionResponse>>> GetRolePermissionsAsync(string roleId, CancellationToken ct = default);
}
