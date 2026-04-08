namespace Application;

public static class DependancyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services) => services
        .AddCQRS()
        .AddServices();

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(DependancyInjection).Assembly);
        services.AddSingleton<IMapper>(new Mapper(config));

        services.AddValidatorsFromAssemblyContaining<AddUserRequest>(includeInternalTypes: true);

        return services;
    }
    private static IServiceCollection AddCQRS(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependancyInjection))
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.Decorate(typeof(ICommandHandler<>), typeof(LoggingDecorator.CommandHandler<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));
        //services.Decorate(typeof(IQueryHandler<,>), typeof(LoggingDecorator.QueryHandler<,>));


        //services.Scan(scan => scan.FromAssembliesOf(typeof(DependancyInjection))
        //    .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
        //        .AsImplementedInterfaces()
        //        .WithScopedLifetime());

        return services;
    }
}
