namespace Domain.Errors;

public static class DepartmentErrors
{
    private const string _code = "departments";

    public static Error DuplicatedName(string name)
        => Error.BadRequest(
        $"{_code}.{nameof(DuplicatedName)}", 
        $"Department with name '{name}' already exists.");

    public static Error NotFound
        => Error.NotFound(
        $"{_code}.{nameof(NotFound)}", 
        $"Department was not found.");
    
    public static Error AlreadyInDepartment
        => Error.BadRequest(
        $"{_code}.{nameof(AlreadyInDepartment)}",
        $"User is already in a department.");

    public static Error UserNotInDepartment
        => Error.BadRequest(
        $"{_code}.{nameof(UserNotInDepartment)}",
        $"User is not in the specified department.");
}
