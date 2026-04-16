namespace Application.Features.Auths.Commands.Add;

public sealed record LoginCommand(LoginRequest Request) : ICommand<AuthResponse>;

public sealed class LoginCommandHandler(
    ILogger<LoginCommandHandler> logger,
    IAuthService authService) : ICommandHandler<LoginCommand, AuthResponse>
{
    public async Task<Result<AuthResponse>> HandleAsync(LoginCommand command, CancellationToken ct = default)
    {
        try
        {
            var authResponse = await authService.GetTokenAsync(command.Request, ct);

            if (authResponse.IsFailure)
            {
                logger.LogError(
                    "Login failed for user {Username}. Error: {Error}",
                    command.Request.Email, authResponse.Error);
                return authResponse.Error;
            }

            logger.LogInformation(
                "User {Username} logged in successfully. Token expires at {Expiration}",
                command.Request.Email, authResponse.Value!.Expiration);

            return authResponse;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while logging in user {Username}",
                command.Request.Email
            );

            return Error.Unexpected("An error occurred while logging in.");
        }
    }
}