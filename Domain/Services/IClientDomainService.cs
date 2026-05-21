namespace Domain.Services;

public interface IClientDomainService
{
    Result<Guid> Create(string userId, string address, string? notes = null);
    Task<Result> UpdateAsync(
        Guid id, string userId, string? firstName, 
        string? lastName, string? address, 
        string? notes, CancellationToken ct = default);
    Task<Result> ActiveAsync(Guid id, CancellationToken ct = default);
    Task<Result> DeactivaAsync(Guid id, string userId, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, string userId, CancellationToken ct = default);
}