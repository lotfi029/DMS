using Application.DTOs.Roles;

namespace Infrastructure.Services;

internal sealed class RoleService(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager
) : IRoleService
{
    public async Task<Result> CreateRoleAsync(string roleName, CancellationToken ct = default)
    {
        if (await roleManager.RoleExistsAsync(roleName))
            return Error.BadRequest("Role.Create", "Role already exists");
        var result = await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
        return result.Succeeded ? Result.Success() : Error.BadRequest("Role.Create", string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<Result> DeleteRoleAsync(string roleId, CancellationToken ct = default)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role is null)
            return Error.NotFound("Role.Delete", "Role not found");
        var result = await roleManager.DeleteAsync(role);
        return result.Succeeded ? Result.Success() : Error.BadRequest("Role.Delete", string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<Result> UpdateRoleAsync(string roleId, string newRoleName, CancellationToken ct = default)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role is null)
            return Error.NotFound("Role.Update", "Role not found");
        role.Name = newRoleName;
        var result = await roleManager.UpdateAsync(role);
        return result.Succeeded ? Result.Success() : Error.BadRequest("Role.Update", string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<Result> AssignRoleToUserAsync(string userId, string roleId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        var role = await roleManager.FindByIdAsync(roleId);
        if (user is null || role is null)
            return Error.NotFound("Role.AssignToUser", "User or Role not found");
        var result = await userManager.AddToRoleAsync(user, role.Name!);
        return result.Succeeded ? Result.Success() : Error.BadRequest("Role.AssignToUser", string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<Result> RemoveRoleFromUserAsync(string userId, string roleId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        var role = await roleManager.FindByIdAsync(roleId);
        if (user is null || role is null)
            return Error.NotFound("Role.RemoveFromUser", "User or Role not found");
        var result = await userManager.RemoveFromRoleAsync(user, role.Name!);
        return result.Succeeded ? Result.Success() : Error.BadRequest("Role.RemoveFromUser", string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<Result<IEnumerable<RoleResponse>>> GetUserRolesAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Error.NotFound("Role.GetUserRoles", "User not found");
        
        var roles = await dbContext.Roles
            .Join(dbContext.UserRoles.Where(ur => ur.UserId == userId), r => r.Id, ur => ur.RoleId, (r, ur) => new RoleResponse(r.Id, r.Name!))
            .ToListAsync(ct);

        return roles;
    }

    public async Task<Result<IEnumerable<RoleResponse>>> GetAllRolesAsync(CancellationToken ct = default)
    {
        var roles = await roleManager.Roles.Select(r => new RoleResponse(r.Id, r.Name!)).ToListAsync(ct);
        return roles;
    }
}
