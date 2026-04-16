using Infrastructure.Persistence.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
    );


builder.Services
    .AddApi()
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddDomain();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Policy1",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.ApplyMigrations();
await app.SeedDataAsync();

app.UseHttpsRedirection();

app.UseCors("Policy1");
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.MapEndpoints();
app.Run();
