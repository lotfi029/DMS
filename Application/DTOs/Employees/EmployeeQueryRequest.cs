namespace Application.DTOs.Employees;

public record EmployeeQueryRequest(
    int Page,
    int PageSize,
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