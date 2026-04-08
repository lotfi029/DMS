using Microsoft.AspNetCore.Builder;

namespace Infrastructure.Persistence.Seeders;

public static class SeederManager
{
    public static IServiceCollection RegisterSeeders(this IServiceCollection services)
    {
        services.AddScoped<RoleSeeder>();
        services.AddScoped<UserSeeder>();
        services.AddScoped<RoleClaimSeeder>();

        return services;
    }

    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        var roleSeeder =
            scope.ServiceProvider.GetRequiredService<RoleSeeder>();
        var userSeeder =
            scope.ServiceProvider.GetRequiredService<UserSeeder>();
        var roleClaimSeeder =
            scope.ServiceProvider.GetRequiredService<RoleClaimSeeder>();


        await roleSeeder.SeedingRolesAsync();
        await roleClaimSeeder.SeedingRoleClaimsAsync();
        await userSeeder.SeedingUsersAsync();
    }
}
