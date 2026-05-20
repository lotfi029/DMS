using API.Infrastructure;

namespace API;

public static class DependancyInjection
{
    public static IServiceCollection AddApi(
        this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
        services.AddEndpoints(typeof(Program).Assembly);
        return services;
    }

}
