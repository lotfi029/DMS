namespace Domain.IRepositories;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<string> GetDepartmentNameAsync(Guid Id, CancellationToken ct = default);
    Task<int> GetEmployeeCountAsync(Guid Id, CancellationToken ct = default);
}
