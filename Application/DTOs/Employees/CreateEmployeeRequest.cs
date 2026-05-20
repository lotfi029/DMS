namespace Application.DTOs.Employees;

public sealed record CreateEmployeeRequest(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string JobTitle,
    string RoleId, 
    ContractType ContractType,
    Guid? DepartmentId,
    string? PhoneNumber = null,
    string? EmergencyContactName = null,
    string? EmergencyContactPhone = null,
    string? Notes = null,
    IEnumerable<string> GrantPermissions = default!,
    IEnumerable<string> DenyPermissions = default!
    );
