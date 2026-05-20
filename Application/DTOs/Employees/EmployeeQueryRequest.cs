namespace Application.DTOs.Employees;

public record EmployeeQueryRequest(
    string? JobTitle,
    IEnumerable<string>? RoleIds,
    IEnumerable<Guid>? DepartmentIds,
    bool? IsActive,
    DateOnly? HireDateMin,
    DateOnly? HireDateMax,
    DateTime? LastLoginDateMin,
    DateTime? LastLoginDateMax,
    DateTime? CreatedAtMin,
    DateTime? CreatedAtMax,
    UserType? UserType
    );