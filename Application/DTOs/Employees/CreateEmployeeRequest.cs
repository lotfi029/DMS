namespace Application.DTOs.Employees;

public sealed record CreateEmployeeRequest(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string JobTitle,
    string? RoleId,
    Guid? DepartmentId,
    string? Notes
    );