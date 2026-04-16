using Application.DTOs.Roles;

namespace Application.Interfaces;

public interface IRoleService
{
    public Task<Result> CreateRoleAsync(string roleName, CancellationToken ct = default);
    public Task<Result> DeleteRoleAsync(string roleId, CancellationToken ct = default);
    public Task<Result> UpdateRoleAsync(string roleId, string newRoleName, CancellationToken ct = default);
    public Task<Result> AssignRoleToUserAsync(string userId, string roleId, CancellationToken ct = default);
    public Task<Result> RemoveRoleFromUserAsync(string userId, string roleId, CancellationToken ct = default);
    public Task<Result<IEnumerable<RoleResponse>>> GetUserRolesAsync(string userId, CancellationToken ct = default);
    public Task<Result<IEnumerable<RoleResponse>>> GetAllRolesAsync(CancellationToken ct = default);
}