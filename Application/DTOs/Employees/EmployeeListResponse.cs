namespace Application.DTOs.Employees;

public sealed record EmployeeListResponse(
    Guid Id,
    string UserId,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string JobTitle,
    bool IsActive,
    IEnumerable<DepartmentFromEmployeeResponse> Departments
);