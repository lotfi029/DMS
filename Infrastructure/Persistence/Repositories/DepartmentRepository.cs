namespace Infrastructure.Persistence.Repositories;

internal sealed class DepartmentRepository(ApplicationDbContext dbContext) 
    : GenericRepository<Department>(dbContext), IDepartmentRepository
{
    public async Task<int> GetEmployeeCountAsync(Guid Id, CancellationToken ct = default)
    {
        var cnt = await DbContext.EmployeeDepartments
            .CountAsync(e => e.DepartmentId == Id, ct);

        return cnt;
    }
    public async Task<string> GetDepartmentNameAsync(Guid Id, CancellationToken ct = default)
    {
        var cnt = await DbContext.Departments
            .Where(d => d.Id == Id)
            .Select(d => d.Name)
            .FirstOrDefaultAsync(ct);

        return cnt ?? "No Department Found";
    }
}
