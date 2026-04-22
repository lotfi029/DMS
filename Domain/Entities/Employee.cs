namespace Domain.Entities;

public sealed class Employee : Entity, IAuditable
{
    public string JobTitle { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateOnly HireDate { get; set; }
    public string? Notes { get; set; }

    public string AppUserId { get; set; } = string.Empty;
    public ApplicationUser AppUser { get; set; } = default!;
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

    private Employee() { }

    private Employee(
        string jobTitle,
        string appUserId,
        DateOnly hireDate,
        string? notes = null,
        Guid? departmentId = null)
    {
        JobTitle = jobTitle;
        HireDate = hireDate;
        Notes = notes;
        AppUserId = appUserId;
        DepartmentId = departmentId;
        IsActive = true;
    }

    public static Employee Create(
        string jobTitle,
        string appUserId,
        DateOnly hireDate,
        string? notes = null,
        Guid? departmentId = null)
    {
        return new Employee(
            jobTitle: jobTitle,
            appUserId: appUserId,
            hireDate: hireDate,
            notes: notes,
            departmentId: departmentId);
    }
}