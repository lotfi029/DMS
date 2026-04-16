namespace Domain.Errors;

public static class UserErrors
{
    private const string _code = "users";
    public static Error NotFound
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}", 
            $"User was not found.");
}
