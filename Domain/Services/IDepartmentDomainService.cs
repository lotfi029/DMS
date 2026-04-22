namespace Domain.Services;

public interface IDepartmentDomainService
{
    Task<Result<Guid>> CreateAsync(string name, string? description, CancellationToken ct = default);
    Task<Result> AddUserAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string name, string? description, CancellationToken ct = default);
    Task<Result> MoveUserAsync(Guid employeeId, Guid newDepartmentId, CancellationToken ct = default);
    Task<Result> RemoveUserAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);

    Task<Result<Department>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<Department>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<Employee>>> GetUsersAsync(Expression<Func<Employee, bool>> predicate, CancellationToken ct = default);
}
