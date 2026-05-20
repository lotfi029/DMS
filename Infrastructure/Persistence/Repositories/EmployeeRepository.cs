namespace Infrastructure.Persistence.Repositories;

internal sealed class EmployeeRepository(ApplicationDbContext dbContext) 
    : GenericRepository<Employee>(dbContext), IEmployeeRepository
{
    public async Task<IEnumerable<Employee>> GetEmployeesByRoleAsync(string roleId, CancellationToken ct = default)
    {
        var employees = await DbContext.UserRoles
            .Where(ur => ur.RoleId == roleId)
            .Select(ur => ur.UserId)
            .ToListAsync(ct);

        return await DbContext.Employees
            .Where(e => employees.Contains(e.AppUserId))
            .Include(x => x.AppUser)
            .ToListAsync(ct);
    }
}