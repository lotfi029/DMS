namespace Domain.Services;

public interface IEmployeeDomainService
{
    Task<Result> ActivateAsync(Guid id, CancellationToken ct = default);
    Result<Guid> Create(
        string userId,
        string jobTitle,
        ContractType contractType,
        string? phoneNumber = null,
        string? emergencyContactName = null,
        string? emergencyContactPhone = null,
        string? notes = null);
    Task<Result> DeactivateAsync(Guid id, CancellationToken ct = default);
    Task<Result> UpdateAsync(
        Guid id,
        string? firstName,
        string? lastName,
        string? jobTitle = null,
        ContractType? contractType = null,
        string? phoneNumber = null,
        string? emergencyContactName = null,
        string? emergencyContactPhone = null,
        string? notes = null,
        CancellationToken ct = default);
}