namespace Application.Features.Auths.Commands.Revoke;


public sealed record RevokeRefreshTokenCommand(RefreshTokenRequest Request) : ICommand;

public sealed class RevokeRefreshTokenCommandHandler(
    ILogger<RevokeRefreshTokenCommandHandler> logger,
    IAuthService authService) : ICommandHandler<RevokeRefreshTokenCommand>
{
    public async Task<Result> HandleAsync(RevokeRefreshTokenCommand command, CancellationToken ct = default)
    {
        try
        {
            var result = await authService.RevokeRefreshToken(command.Request, ct);
            if (result.IsFailure)
            {
                logger.LogError(
                    "Revoke refresh token failed for token {Token}. Error: {Error}",
                    command.Request.Token, result.Error);
                return result.Error;
            }
            logger.LogInformation(
                "Revoke refresh token successful for token {Token}",
                command.Request.Token);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while revoking refresh token {Token}",
                command.Request.Token
            );
            return Error.Unexpected("An error occurred while revoking the refresh token.");
        }
    }
}