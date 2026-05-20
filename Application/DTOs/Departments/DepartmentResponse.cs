namespace Application.DTOs.Departments;

public sealed record DepartmentResponse(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    Guid? DepartmentHeadId,
    string? DepartmentHeadName,
    int EmployeeCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);