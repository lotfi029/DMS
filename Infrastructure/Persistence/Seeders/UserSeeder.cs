namespace Infrastructure.Persistence.Seeders;

public sealed class UserSeeder(
    UserManager<ApplicationUser> userManager,
    ILogger<UserSeeder> logger)
{
    public async Task SeedingUsersAsync()
    {
        foreach(var user in DefaultUsers.All)
        {
            if (await userManager.FindByIdAsync(user.Id) is not null)
            {
                logger.LogInformation("User '{UserName}' already exists. Skipping.", user.UserName);
                continue;
            }

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                logger.LogInformation("User '{UserName}' created successfully.", user.UserName);

                await userManager.AddToRoleAsync(user, DefaultRoles.Manager.Name!);
                
                logger.LogInformation("User '{UserName}' added to role '{RoleName}' successfully.", 
                    user.UserName, 
                    DefaultRoles.Manager.Name);
            }
            else
            {
                logger.LogError("Failed to create user '{UserName}'. Errors: {Errors}", 
                    user.UserName, 
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}