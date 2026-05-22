namespace Application.DTOs.Employees;

public sealed record DepartmentFromEmployeeResponse(
    Guid Id,
    string Name
);