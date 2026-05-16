namespace Domain.Entities;

public class Department: Entity, IAuditable
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public Guid? DepartmentHeadId { get; private set; }
    public Employee? DepartmentHead { get; private set; }
    public ICollection<EmployeeDepartment> EmployeeDepartments { get; set; } = [];
    private Department() { }

    public static Department Create(
        string name, string? description = null)
    {
        return new Department
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
    public void AssignHead(Guid headId)
    {
        DepartmentHeadId = headId;
    }

    public void RemoveHead() => DepartmentHeadId = null;


    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
