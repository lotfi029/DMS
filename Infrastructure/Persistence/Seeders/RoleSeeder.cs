namespace Infrastructure.Persistence.Seeders;

public sealed class RoleSeeder(
    RoleManager<ApplicationRole> roleManager,
    ILogger<RoleSeeder> logger)
{
    public async Task SeedingRolesAsync()
    {
        foreach(var role in DefaultRoles.All)
        {
            if (await roleManager.RoleExistsAsync(role.Name!))
            {
                logger.LogInformation("Role '{RoleName}' already exists. Skipping.", role.Name);
                continue;
            }
            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                logger.LogInformation("Role '{RoleName}' created successfully.", role.Name);
            }
            else
            {
                logger.LogError("Failed to create role '{RoleName}'. Errors: {Errors}", 
                    role.Name, 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}