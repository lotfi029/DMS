namespace Domain.Errors;

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

    public static Error AlreadyActive
        => Error.Conflict(
            $"{_code}.{nameof(AlreadyActive)}",
            $"Employee is already active.");

    public static Error InactiveException
        => Error.Conflict(
            $"{_code}.{nameof(InactiveException)}",
            $"Error occure will inactive employee.");
    public static Error ActiveException
        => Error.Conflict(
            $"{_code}.{nameof(ActiveException)}",
            $"Error occure will active employee.");
}