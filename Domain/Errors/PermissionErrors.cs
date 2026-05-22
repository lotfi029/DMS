namespace Domain.Errors;

public static class PermissionErrors
{
    private const string _code = "permissions";
    public static Error PermissionOverrideNotFound
        => Error.NotFound(
            $"{_code}.{nameof(PermissionOverrideNotFound)}",
            "The specified permission override was not found.");
    public static Error InvalidPermission(string permission)
        => Error.BadRequest(
            $"{_code}.{nameof(InvalidPermission)}",
            $"The permission '{permission}' is not a valid permission.");

    public static Error NotFoundForTarget(string permission)
        => Error.NotFound(
            $"{_code}.{nameof(NotFoundForTarget)}",
            $"Permission '{permission}' was not found for the specified target.");

    public static Error PermissionOverrideAlreadyExists(string permission)
        => Error.Conflict(
            $"{_code}.{nameof(PermissionOverrideAlreadyExists)}",
            $"A permission override for '{permission}' already exists for the specified target.");
}