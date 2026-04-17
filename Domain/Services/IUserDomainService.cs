namespace Domain.Services;

public interface IUserDomainService
{
    Task<Result> UpdateAsync(string userId, string firstName, string lastName, CancellationToken ct = default);
    Task<Result> DeactivateAsync(string userId, CancellationToken ct = default);
    Task<Result> ActivateAsync(string userId, CancellationToken ct = default);
    Task<Result<ApplicationUser>> GetByIdAsync(string userId, CancellationToken ct = default);
    Task<Result<IEnumerable<ApplicationUser>>> GetAllAsync(string userId, CancellationToken ct = default);
    Task<Result> UpdateLastLoginAsync(string userId, CancellationToken ct = default);
}
