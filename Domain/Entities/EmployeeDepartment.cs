namespace Domain.Entities;

public sealed class EmployeeDepartment : IAuditable
{
    public Guid EmployeeId { get; set; }
    public Guid DepartmentId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

    public Employee Employee { get; set; } = null!;
    public Department Department { get; set; } = null!;
    private EmployeeDepartment() { }
    private EmployeeDepartment(Guid employeeId, Guid departmentId)
    {
        EmployeeId = employeeId;
        DepartmentId = departmentId;
    }

    public static EmployeeDepartment Create(Guid employeeId, Guid departmentId)
    {
        return new EmployeeDepartment(employeeId, departmentId);
    }
}