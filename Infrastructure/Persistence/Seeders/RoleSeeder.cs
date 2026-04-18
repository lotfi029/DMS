namespace Infrastructure.Persistence.Seeders;

internal sealed class RoleSeeder(
    RoleManager<ApplicationRole> roleManager,
    ILogger<RoleSeeder> logger)
{
    public async Task SeedingRolesAsync()
    {
        foreach (var role in DefaultRoles.All)
        {
            if (await roleManager.RoleExistsAsync(role.Name!))
            {
                logger.LogInformation(LogMessages.DB_SeedSkipped, "Role", role.Name);
                continue;
            }

            logger.LogInformation(LogMessages.DB_SeedStarted, $"Role:{role.Name}");

            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
                logger.LogInformation(LogMessages.DB_SeedCompleted, $"Role:{role.Name}");
            else
                logger.LogError("Failed to seed role '{RoleName}'. Errors: {Errors}",
                    role.Name, string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}