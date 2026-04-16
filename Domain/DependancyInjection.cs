namespace Domain;

public static class DependancyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.RegisterDomainServices();
        return services;
    }
    private static void RegisterDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IDepartmentDomainService, DepartmentDomainService>();
        services.AddScoped<IUserDomainService, UserDomainService>();
    }
}
