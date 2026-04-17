using Application.DTOs.Permissions;

namespace Infrastructure.Services;

internal sealed class PermissionService(
    ApplicationDbContext dbContext,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager
    ) : IPermissionService
{
    public async Task<Result<IEnumerable<PermissionResponse>>> GetUserPermissionsAsync(string userId, CancellationToken ct = default)
    {
        if (await userManager.FindByIdAsync(userId) is not { } user)
            return Error.NotFound("Permission.GetUserPermissions", "User not found");

        var roles = await userManager.GetRolesAsync(user);

        var permissions = await dbContext.Roles
            .Where(r => roles.Contains(r.Name!))
            .Join(dbContext.RoleClaims.Where(c => c.ClaimType == DefaultPermissions.ClaimType), r => r.Id, c => c.RoleId, (r, c) => new PermissionResponse(c.Id, r.Id, c.Group, c.ClaimValue!, c.DisplayName, c.Description))
            .Distinct()
            .ToListAsync(ct);


        return permissions!;
    }

    public async Task<Result<IEnumerable<PermissionResponse>>> GetAllPermissionAsync(CancellationToken ct = default)
    {
        var permissions = await dbContext.RoleClaims
            .Where(c => c.ClaimType == DefaultPermissions.ClaimType)
            .ProjectToType<PermissionResponse>()
            .Distinct()
            .ToListAsync(ct);
        
        return permissions!;
    }

    public async Task<Result> AssignPermissionToRoleAsync(string roleId, string permission, CancellationToken ct = default)
    {
        if (await roleManager.FindByIdAsync(roleId) is not { } role )
            return Error.NotFound("Permission.AssignToRole", "Role not found");

        if (await dbContext.RoleClaims.AnyAsync(c => c.RoleId == roleId && c.ClaimType == DefaultPermissions.ClaimType && c.ClaimValue == permission, ct))
            return Error.BadRequest("Permission.AssignToRole", "Permission already assigned to role");
        
        
        dbContext.RoleClaims.Add(ApplicationRoleClaim.Create(roleId, DefaultPermissions.ClaimType, permission));
        await dbContext.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> RemovePermissionFromRoleAsync(string roleId, string permission, CancellationToken ct = default)
    {
        var claims = dbContext.RoleClaims.Where(c => c.RoleId == roleId && c.ClaimType == DefaultPermissions.ClaimType  && c.ClaimValue == permission);
        dbContext.RoleClaims.RemoveRange(claims);
        await dbContext.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<PermissionResponse>>> GetRolePermissionsAsync(string roleId, CancellationToken ct = default)
    {
        var permissions = await dbContext.RoleClaims
            .Where(c => c.RoleId == roleId && c.ClaimType == DefaultPermissions.ClaimType)
            .ProjectToType<PermissionResponse>()
            .ToListAsync(ct);

        return permissions!;
    }
}
