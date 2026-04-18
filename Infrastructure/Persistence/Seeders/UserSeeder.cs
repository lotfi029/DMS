namespace Infrastructure.Persistence.Seeders;

public sealed class UserSeeder(
    UserManager<ApplicationUser> userManager,
    ILogger<UserSeeder> logger)
{
    public async Task SeedingUsersAsync()
    {
        foreach (var user in DefaultUsers.All)
        {
            if (await userManager.FindByIdAsync(user.Id) is not null)
            {
                logger.LogInformation(LogMessages.DB_SeedSkipped, "User", user.UserName);
                continue;
            }

            logger.LogInformation(LogMessages.DB_SeedStarted, $"User:{user.UserName}");

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                logger.LogInformation(LogMessages.DB_SeedCompleted, $"User:{user.UserName}");
                await userManager.AddToRoleAsync(user, DefaultRoles.Manager.Name!);
                logger.LogInformation(
                    "User '{UserName}' assigned to role '{RoleName}'",
                    user.UserName, DefaultRoles.Manager.Name);
            }
            else
            {
                logger.LogError("Failed to seed user '{UserName}'. Errors: {Errors}",
                    user.UserName, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}