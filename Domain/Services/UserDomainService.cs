namespace Domain.Services;

internal sealed class UserDomainService(
    IUserRepository userRepository) : IUserDomainService
{
    public async Task<Result> ActivateAsync(string userId, CancellationToken ct = default)
    {
        var rowsAffected = await userRepository
            .ExecuteUpdateAsync(
                u => u.Id == userId && !u.IsActive,
                u => u.SetProperty(p => p.IsActive, true), 
            ct);

        if (rowsAffected == 0)
            return UserErrors.NotFound;

        return Result.Success();
    }
    public async Task<Result> DeactivateAsync(string userId, CancellationToken ct = default)
    {
        var rowsAffected = await userRepository
            .ExecuteUpdateAsync(
                u => u.Id == userId && u.IsActive,
                u => u.SetProperty(p => p.IsActive, false),
            ct);

        if (rowsAffected == 0)
            return UserErrors.NotFound;

        return Result.Success();
    }

    public async Task<Result<IEnumerable<ApplicationUser>>> GetAllAsync(string userId, CancellationToken ct = default)
    {
        var users = await userRepository.GetAllAsync(x => x.Id != userId, ct);

        return Result.Success(users);
    }

    public async Task<Result<ApplicationUser>> GetByIdAsync(string userId, CancellationToken ct = default)
    {
        if (await  userRepository.GetByIdAsync(x => x.Id == userId,ct) is not { } user)
            return UserErrors.NotFound;

        return user;
    }

    public async Task<Result> UpdateAsync(string userId, string firstName, string lastName, CancellationToken ct = default)
    {
        var rowsAffected = await userRepository
            .ExecuteUpdateAsync(
                u => u.Id == userId,
                u => u.SetProperty(p => p.FirstName, firstName)
                    .SetProperty(p => p.LastName, lastName), ct);
        if (rowsAffected == 0)
            return UserErrors.NotFound;

        return Result.Success();
    }
}