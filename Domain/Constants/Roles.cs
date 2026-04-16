namespace Domain.Constants;

public static class DefaultRoles
{
    public static readonly ApplicationRole Admin = new()
    {
        Id = "652ad2aa-6ee5-4bf0-a512-38004794f1fa",
        Name = "admin",
        NormalizedName = "ADMIN",
        Description = "System role: admin.",
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };
    public static readonly ApplicationRole Manager = new()
    {
        Id = "0e5f027e-5578-4f15-968d-c86481600bab",
        Name = "manager",
        NormalizedName = "MANAGER",
        Description = "System role: manager",
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };
    public static readonly ApplicationRole DepartmentHead = new() 
    {
        Id = "e17fd8ab-91b2-40da-8dbc-c4eb9ce646ff",
        Name = "DepartmentHead",
        NormalizedName = "DEPARTMENTHEAD",
        Description = "System role: department-head",
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };
    public static readonly ApplicationRole Employee = new()
    {
        Id = "e1dc8596-0d07-40fd-952a-5dde637a0f22",
        Name = "Employee",
        NormalizedName = "EMPLOYEE",
        Description = "System role: employee",
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };

    public static readonly IList<ApplicationRole> All = 
        [ Admin, DepartmentHead, Employee, Manager];
}
