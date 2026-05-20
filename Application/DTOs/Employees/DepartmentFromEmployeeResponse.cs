namespace Application.DTOs.Employees;

public sealed record DepartmentFromEmployeeResponse(
    Guid Id,
    string Name
);
public sealed record RoleForEmployeeResponse(
    string Id,
    string Name
);