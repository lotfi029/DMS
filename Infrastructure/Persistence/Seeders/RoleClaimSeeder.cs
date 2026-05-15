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
                var exceptClaim = permissions.Except(existingClaims.Select(c => c.Value));
                if (!exceptClaim.Any())
                    logger.LogInformation(LogMessages.DB_SeedSkipped, "RoleClaims", role.Name);
                else
                {
                    var newClaims = exceptClaim.Select(p =>
                        ApplicationRoleClaim.Create(role.Id, DefaultPermissions.ClaimType, p));
                    dbContext.RoleClaims.AddRange(newClaims);
                    logger.LogInformation(LogMessages.DB_SeedUpdated, $"RoleClaims: {role.Name}");
                    await dbContext.SaveChangesAsync();
                }
                continue;
            }

            logger.LogInformation(LogMessages.DB_SeedStarted, $"RoleClaims: {role.Name}");

            var claims = permissions.Select(p =>
                ApplicationRoleClaim.Create(role.Id, DefaultPermissions.ClaimType, p));

            dbContext.RoleClaims.AddRange(claims);
            await dbContext.SaveChangesAsync();

            logger.LogInformation(LogMessages.DB_SeedCompleted, $"RoleClaims: {role.Name}");
        }
    }
}