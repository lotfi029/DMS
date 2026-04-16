namespace Domain.Services;

public interface IDepartmentDomainService
{
    Task<Result> CreateAsync(string name, string? description, CancellationToken ct = default);
    Task<Result> AddUserAsync(string userId, Guid departmentId, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string name, string? description, CancellationToken ct = default);
    Task<Result> MoveUserAsync(string userId, Guid newDepartmentId, CancellationToken ct = default);
    Task<Result> RemoveUserAsync(string userId, Guid departmentId, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);

    Task<Result<Department>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<Department>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IEnumerable<ApplicationUser>>> GetUsersAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken ct = default);
}
