namespace Domain.Services;

public interface IEmployeeDomainService
{
    Task<Result> ActivateAsync(Guid id, CancellationToken ct = default);
    Result<Guid> Create(string userId, string jobTitle, Guid departmentId, string? notes = null);
    Task<Result> DeactivateAsync(Guid id, CancellationToken ct = default);
    Task<Result<IEnumerable<Employee>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<Employee>> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string? firstName, string? lastName, string? jobTitle = null, string? notes = null, CancellationToken ct = default);
}