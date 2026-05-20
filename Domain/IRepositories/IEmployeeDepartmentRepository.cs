namespace Domain.IRepositories;

public interface IEmployeeDepartmentRepository : IGenericRepository<EmployeeDepartment>
{
    Task<IEnumerable<EmployeeDepartment>> GetDepartmentAsync(Guid employeeId, CancellationToken ct = default);
    Task<IEnumerable<EmployeeDepartment>> GetEmployeeAsync(Guid departmentId, CancellationToken ct = default);
}