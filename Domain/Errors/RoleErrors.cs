namespace Domain.Errors;

public static class RoleErrors
{
    private const string _code = "roles";

    public static Error Unauthorized
        => Error.Unauthorized(
            $"{_code}.{nameof(Unauthorized)}",
            $"Unauthorized.");

    public static Error DuplicatedRole(string roleName)
        => Error.BadRequest(
            $"{_code}.{nameof(DuplicatedRole)}",
            $"Role '{roleName}' is already taken.");

    public static Error NotFound
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}",
            $"Role was not found.");
}