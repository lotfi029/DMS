namespace Application.DTOs.Employees;

public sealed record EmployeeListResponse(
    Guid Id,
    string AppUserId,
    string FirstName,
    string LastName,
    string Email,
    string JobTitle,
    bool IsActive,
    Guid? DepartmentId,
    string? DepartmentName
    );