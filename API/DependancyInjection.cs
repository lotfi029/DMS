namespace API;

public static class DependancyInjection
{
    public static IServiceCollection AddApi(
        this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });
        services.AddEndpoints(typeof(Program).Assembly);
        return services;
    }

}
