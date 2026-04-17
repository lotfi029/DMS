namespace Infrastructure.Persistence.Seeders;

public sealed class RoleClaimSeeder(
    RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext dbContext,
    ILogger<RoleClaimSeeder> logger)
{
    public async Task SeedingRoleClaimsAsync()
    {
        foreach(var role in RolePermissions.DefaultRolePermissions)
        {
            if (await roleManager.RoleExistsAsync(role.Key.Name!))
            {
                // TODO:
                if (await roleManager.GetClaimsAsync(role.Key) is { } claims && claims.Any())
                    continue;
                
                
                var addedClaims = role.Value.Select(claim => ApplicationRoleClaim.Create(role.Key.Id, DefaultPermissions.ClaimType, claim));

                dbContext.RoleClaims.AddRange(addedClaims);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Claims for role '{RoleName}' added successfully.", role.Key.Name);
            }
        }
    }
}