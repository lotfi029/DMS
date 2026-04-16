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
}
