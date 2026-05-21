namespace Domain.ReadModels;

public sealed class EmployeeProfileView
{
    public Guid EmployeeId { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? UserName { get; init; }
    public string? UserPhone { get; init; }
    public bool UserIsActive { get; init; }
    public string RoleName { get; init; } = string.Empty;
    public string UserRoleId { get; init; } = string.Empty;
    public DateTime? LastLoginAt { get; init; }
    public DateTime CreatedAt { get; init; }
    //public string? ProfilePictureUrl { get; init; }
    public string JobTitle { get; init; } = string.Empty;
    //public int ContractType { get; init; }
    //public int? MomsRole { get; init; }
    //public decimal? BasicSalary { get; init; }
    public string? EmployeePhone { get; init; }
    public string? EmergencyContactName { get; init; }
    public string? EmergencyContactPhone { get; init; }
    public DateOnly HireDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public bool EmployeeIsActive { get; init; }
    public string? Notes { get; init; }
    public DateTime EmployeeCreatedAt { get; init; }
    //public Guid TenantId { get; init; }
    public Guid? DepartmentId { get; init; }
    public string? DepartmentName { get; init; }
}