namespace Application.DTOs.Employees;

public sealed record EmployeeResponse(
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


public sealed record DetailedEmployeeResponse(
    Guid Id,
    string AppUserId,
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string JobTitle,
    DateOnly HireDate,
    bool IsActive,
    DateTime CreatedAt,
    DateTime LastLoginAt,
    string? Notes,
    Guid? DepartmentId,
    string? DepartmentName
    );