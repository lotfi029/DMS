namespace Domain.Errors;

public static class UserErrors
{
    private const string _code = "users";
    public static Error NotFound
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}", 
            $"User was not found.");
}
public static class EmployeeErrors
{
    private const string _code = "employees";

    public static Error NotFound
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}",
            $"Employee was not found.");

    public static Error AlreadyInactive
        => Error.Conflict(
            $"{_code}.{nameof(AlreadyInactive)}",
            $"Employee is already inactive.");
}