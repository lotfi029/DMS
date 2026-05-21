using Domain.ReadModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Employee> Employees { get; }
    DbSet<Department> Departments { get; }
    DbSet<EmployeeDepartment> EmployeeDepartments { get; }
    DbSet<ApplicationUser> Users { get; }
    DbSet<ApplicationRole> Roles { get; }
    DbSet<IdentityUserRole<string>> UserRoles { get; }
    DbSet<EmployeeProfileView> EmployeeProfiles { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
