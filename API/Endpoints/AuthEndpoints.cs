using Application.Abstractions.Messaging;
using Application.DTOs.Auths;
using Application.Features.Auths.Commands.Add;
using Application.Features.Auths.Commands.RefreshToken;
using Application.Features.Auths.Commands.Revoke;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints;

internal sealed class AuthEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Auth");

        group.MapPost("/login", LoginAsync);
        group.MapPost("/refresh-token", RefreshTokenAsync);
        group.MapPost("/revoke-refresh-token", RevokeRefreshTokenAsync);

    }

    private async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        [FromServices] IValidator<LoginRequest> validator,
        [FromServices] ICommandHandler<LoginCommand, AuthResponse> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var command = new LoginCommand(request);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> RefreshTokenAsync(
        [FromBody] RefreshTokenRequest request,
        [FromServices] IValidator<RefreshTokenRequest> validator,
        [FromServices] ICommandHandler<RefreshTokenCommand, AuthResponse> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());
        var command = new RefreshTokenCommand(request);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblem();
    }

    private async Task<IResult> RevokeRefreshTokenAsync(
        [FromBody] RefreshTokenRequest request,
        [FromServices] IValidator<RefreshTokenRequest> validator,
        [FromServices] ICommandHandler<RevokeRefreshTokenCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());
        var command = new RevokeRefreshTokenCommand(request);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
}