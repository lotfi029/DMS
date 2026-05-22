namespace Domain.Errors;

public static class AuthErrors
{
    private const string _code = "authentications";

    public static Error UnAuthorization
        => Error.Unauthorized(
            $"{_code}.{nameof(UnAuthorization)}",
            $"Unauthorized.");

    public static Error DuplicatedEmail(string email)
        => Error.BadRequest(
            $"{_code}.{nameof(DuplicatedEmail)}",
            $"Email '{email}' is already taken.");

    public static Error InvalidCredentials
        => Error.Unauthorized(
            $"{_code}.{nameof(InvalidCredentials)}",
            $"Invalid credentials.");

    public static Error UserNotActive
        => Error.Unauthorized(
            $"{_code}.{nameof(UserNotActive)}",
            $"User is not active.");

    public static Error InvalidToken
        => Error.Unauthorized(
            $"{_code}.{nameof(InvalidToken)}",
            $"Invalid token.");

    public static Error InvalidRefreshToken
        => Error.Unauthorized(
            $"{_code}.{nameof(InvalidRefreshToken)}",
            $"Invalid refresh token.");

    public static Error InvalidUser
        => Error.Unauthorized(
            $"{_code}.{nameof(InvalidUser)}",
            $"Invalid user.");

    public static Error InvalidRegister(string description)
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidRegister)}",
            description);
}
