namespace Domain.Services;

public interface IDepartmentDomainService
{
    Task<Result<Guid>> CreateAsync(string name, string? description, CancellationToken ct = default);
    Task<Result> AddEmployeeAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string name, string? description, CancellationToken ct = default);
    Task<Result> MoveEmployeeAsync(Guid employeeId, Guid newDepartmentId, CancellationToken ct = default);
    Task<Result> RemoveEmployeeAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);

    Task<Result<Department>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<Department>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<Employee>>> GetUsersAsync(Expression<Func<Employee, bool>> predicate, CancellationToken ct = default);
}
