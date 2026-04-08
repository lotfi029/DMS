namespace Application.Features.Auths.Commands.RefreshToken;

public sealed record RefreshTokenCommand(RefreshTokenRequest Request) : ICommand<AuthResponse>;

public sealed class RefreshTokenCommandHandler(
    ILogger<RefreshTokenCommandHandler> logger,
    IAuthService authService) : ICommandHandler<RefreshTokenCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> HandleAsync(RefreshTokenCommand command, CancellationToken ct = default)
    {
        try
        {
            var authResponse = await authService.GetRefreshTokenAsync(command.Request, ct);
            if (authResponse.IsFailure)
            {
                logger.LogError(
                    "Refresh token failed for token {Token}. Error: {Error}",
                    command.Request.Token, authResponse.Error);
                return authResponse.Error;
            }
            logger.LogInformation(
                "Refresh token successful for token {Token}. New token expires at {Expiration}",
                command.Request.Token, authResponse.Value!.Expiration);
            return authResponse;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while refreshing token {Token}",
                command.Request.Token
            );
            return Error.Unexpected("An error occurred while refreshing the token.");
        }
    }
}