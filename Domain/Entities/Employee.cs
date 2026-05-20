namespace Domain.Entities;

public sealed class Employee : Entity, IAuditable
{
    public string JobTitle { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public DateOnly HireDate { get; private set; }
    public DateOnly? EndDate { get; private set; } // will replace with contract (entity will added).
    public string? Notes { get; private set; }
    public ContractType? ContractType { get; private set; }
    //public SystemRole? MomsRole { get; private set; }   // ← NEW
    public string? PhoneNumber { get; private set; }
    public string? EmergencyContactName { get; private set; }
    public string? EmergencyContactPhone { get; private set; }
    public string AppUserId { get; private set; } = string.Empty;
    public ApplicationUser AppUser { get; private set; } = default!;
    public ICollection<EmployeeDepartment> EmployeeDepartments { get; private set; } = [];
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? LastModifiedAt { get; private set; }

    private Employee() { }

    private Employee(
        string appUserId,
        string jobTitle,
        DateOnly hireDate,
        ContractType contractType,
        //SystemRole? systemRole = null,
        string? phoneNumber = null,
        string? emergencyName = null,
        string? emergencyPhone = null,
        string? notes = null)
    {
        JobTitle = jobTitle;
        HireDate = hireDate;
        Notes = notes;
        AppUserId = appUserId;
        ContractType = contractType;
        //MomsRole = systemRole;
        PhoneNumber = phoneNumber;
        EmergencyContactName = emergencyName;
        EmergencyContactPhone = emergencyPhone;
        AppUserId = appUserId;
        IsActive = true;
    }

    public static Employee Create(
        string jobTitle,
        string appUserId,
        DateOnly hireDate,
        ContractType contractType,
        string? phoneNumber = null,
        string? emergencyName = null,
        string? emergencyPhone = null,

        string? notes = null)
    {
        return new Employee(
            jobTitle: jobTitle,
            appUserId: appUserId,
            hireDate: hireDate,
            contractType: contractType,
            phoneNumber: phoneNumber,
            emergencyName: emergencyName,
            emergencyPhone: emergencyPhone,
            notes: notes);
    }
    public void Update(
        string? jobTitle = null,
        ContractType? contractType = null, 
        string? phoneNumber = null,
        string? emergencyName = null,
        string? emergencyPhone = null,
        string? notes = null,
        DateOnly? endDate = null)
    {
        if (!IsActive) return;
        if (jobTitle is not null) JobTitle = jobTitle;
        if (contractType is not null) ContractType = contractType;
        if (phoneNumber is not null) PhoneNumber = phoneNumber;
        if (emergencyName is not null) EmergencyContactName = emergencyName;
        if (emergencyPhone is not null) EmergencyContactPhone = emergencyPhone;
        if (notes is not null) Notes = notes;
        if (endDate is not null) EndDate = endDate;

        LastModifiedAt = DateTime.UtcNow;
    }
    public void Deactivate() { IsActive = false; LastModifiedAt = DateTime.UtcNow; }
    public void Activate() { IsActive = true; LastModifiedAt = DateTime.UtcNow; }
}
