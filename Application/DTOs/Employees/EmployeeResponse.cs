namespace Application.DTOs.Employees;

public sealed record EmployeeResponse(
    Guid Id,
    string UserId,
    string FirstName,
    string LastName,
    string FullName,
    string Email,
    string UserName,
    string JobTitle,
    string ContractType,
    string? PhoneNumber,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    DateOnly HireDate,
    DateOnly? EndDate,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    string? Notes,
    IEnumerable<DepartmentFromEmployeeResponse> Departments,
    IEnumerable<RoleForEmployeeResponse> Roles
);