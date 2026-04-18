namespace Infrastructure.Persistence.Seeders;

public sealed class RoleClaimSeeder(
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext dbContext,
    ILogger<RoleClaimSeeder> logger)
{
    public async Task SeedingRoleClaimsAsync()
    {
        foreach (var (role, permissions) in RolePermissions.DefaultRolePermissions)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
                continue;

            var existingClaims = await roleManager.GetClaimsAsync(role);
            if (existingClaims.Any())
            {
                logger.LogInformation(LogMessages.DB_SeedSkipped, "RoleClaims", role.Name);
                continue;
            }

            logger.LogInformation(LogMessages.DB_SeedStarted, $"RoleClaims:{role.Name}");

            var claims = permissions.Select(p =>
                ApplicationRoleClaim.Create(role.Id, DefaultPermissions.ClaimType, p));

            dbContext.RoleClaims.AddRange(claims);
            await dbContext.SaveChangesAsync();

            logger.LogInformation(LogMessages.DB_SeedCompleted, $"RoleClaims:{role.Name}");
        }
    }
}