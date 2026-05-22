using API.Infrastructure;

namespace API;

public static class DependancyInjection
{
    public static IServiceCollection AddApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthorization();
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
        services.AddEndpoints(typeof(Program).Assembly);
        services.AddCorsService(configuration);

        return services;
    }
    private static IServiceCollection AddCorsService(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        var allowedOrigins = configuration.GetSection(Cors.SectionName).Get<Cors>()!;

        services.AddCors(options =>
        {
            options.AddPolicy(Policies.AngularFrontendPolicy,
                policy =>
                {
                    policy.WithOrigins(allowedOrigins.AllowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
        });

        return services;
    }

}
