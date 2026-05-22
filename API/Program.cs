using Infrastructure.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
    );

builder.Services
    .AddApi(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddDomain();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.ApplyMigrations();
await app.SeedDataAsync();
app.UseCors(Policies.AngularFrontendPolicy);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseExceptionHandler();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapEndpoints();
app.Run();
