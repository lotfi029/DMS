namespace Domain.Errors;

public static class ClientErrors
{
    private const string _code = "clients";
    public static Error NotFound
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}",
            $"Client was not found.");
}