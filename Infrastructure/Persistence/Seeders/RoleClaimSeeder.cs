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
                if (await roleManager.GetClaimsAsync(role.Key) is { } claimsss && claimsss.Any())
                    continue;

                var cnt = await dbContext.RoleClaims
                    .OrderBy(x => x.Id)
                    .Select(x => x.Id)
                    .LastOrDefaultAsync();

                var claims = role.Value.Select(claim => new IdentityRoleClaim<string>
                {
                    Id = ++cnt,
                    ClaimType = Permissions.ClaimType,
                    ClaimValue = claim,
                    RoleId = role.Key.Id
                });

                dbContext.RoleClaims.AddRange(claims);
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Claims for role '{RoleName}' added successfully.", role.Key.Name);
            }
        }
    }
}