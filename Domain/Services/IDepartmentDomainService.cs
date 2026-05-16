namespace Domain.Services;

public interface IDepartmentDomainService
{
    Task<Result<Guid>> CreateAsync(string name, string? description, CancellationToken ct = default);
    Task<Result> AddEmployeeAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default);
    Task<Result> UpdateAsync(Guid id, string name, string? description, CancellationToken ct = default);
    Task<Result> MoveEmployeeAsync(Guid employeeId, Guid newDepartmentId, CancellationToken ct = default);
    Task<Result> RemoveEmployeeAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<Result> AssignDepartmentHeadAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default);
}
