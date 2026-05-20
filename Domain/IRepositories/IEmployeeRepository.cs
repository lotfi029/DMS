namespace Domain.IRepositories;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<IEnumerable<Employee>> GetEmployeesByRoleAsync(string roleId, CancellationToken ct = default);
}
