namespace Infrastructure.Persistence.Repositories;

internal sealed class EmployeeDepartmentRepository(ApplicationDbContext dbContext) 
    : GenericRepository<EmployeeDepartment>(dbContext), IEmployeeDepartmentRepository
{
    public async Task<IEnumerable<EmployeeDepartment>> GetEmployeeAsync(Guid departmentId, CancellationToken ct = default)
    {
        //var query = await (
        //    from ed in DbContext.EmployeeDepartments
        //    join e in DbContext.Employees
        //    on ed.EmployeeId equals e.Id
        //    join u in DbContext.Users
        //    on e.AppUserId equals u.Id
        //    where ed.DepartmentId == departmentId
        //    select new EmployeeListResponse(
        //        Id: e.Id,
        //        AppUserId: u.Id,
        //        FirstName: u.FirstName,
        //        LastName: u.LastName,
        //        Email: u.Email!,
        //        JobTitle: e.JobTitle,
        //        IsActive: u.IsActive,
        //        DepartmentId: departmentId,
        //        string.Empty
        //        )
        //    ).ToListAsync();

        var query = await DbContext.EmployeeDepartments
            .Include(x => x.Employee)
                .ThenInclude(e => e.AppUser)
            .Include(x => x.Department)
            .Where(ed => ed.DepartmentId == departmentId)
            .ToListAsync(ct);

        return query;
    }

    public async Task<IEnumerable<EmployeeDepartment>> GetDepartmentAsync(Guid employeeId, CancellationToken ct = default)
    {
        var query = await DbContext.EmployeeDepartments
            .Include(x => x.Department)
            .Where(ed => ed.EmployeeId == employeeId)
            .ToListAsync(ct);
        return query;
    }
}