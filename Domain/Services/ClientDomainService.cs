namespace Domain.Services;

public sealed class ClientDomainService(
    IClientRepository clientRepository,
    IUserRepository userRepository) : IClientDomainService
{
    public Result<Guid> Create(
        string userId,
        string address,
        string? notes = null)
    {
        var entity = Client.Create(
            userId: userId,
            address: address,
            notes: notes);

        return Result.Success(entity.Id);
    }

    public async Task<Result> UpdateAsync(
        Guid id,
        string userId,
        string? firstName,
        string? lastName,
        string? address,
        string? notes,
        CancellationToken ct = default)
    {
        var updatedRows = await clientRepository.ExecuteUpdateAsync(
            x => x.Id == id,
            setters => 
            {
                setters.SetProperty(x => x.Address, v => address ?? v.Address);
                setters.SetProperty(x => x.Notes, v => notes ?? v.Notes);
            }, ct);

        if (updatedRows == 0)
            return ClientErrors.NotFound;

        var userUpdatedRows = await userRepository.ExecuteUpdateAsync(
            x => x.Id == userId,
            setters => 
            {
                if (!string.IsNullOrWhiteSpace(firstName))
                    setters = setters.SetProperty(x => x.FirstName, firstName);
                if (!string.IsNullOrWhiteSpace(lastName))
                    setters = setters.SetProperty(x => x.LastName, lastName);
            }, ct);

        return Result.Success();
    }
    public async Task<Result> DeleteAsync(Guid id, string userId, CancellationToken ct = default)
    {
        var affectedRows = await clientRepository.ExecuteUpdateAsync(
            x => x.Id == id,
            x => x.SetProperty(p => p.IsActive, false), ct);

        if (affectedRows == 0)
            return ClientErrors.NotFound;

        var userAffectedRows = await userRepository.ExecuteUpdateAsync(
            x => x.Id == userId,
            x => x.SetProperty(p => p.IsActive, false), ct);

        return Result.Success();
    }

    public async Task<Result> ActiveAsync(Guid id, CancellationToken ct = default)
    {
        var client = await clientRepository.GetByIdAsync(x => x.Id == id, true, ct: ct);
        if (client is null)
            return ClientErrors.NotFound;

        if (client.IsActive)
            return ClientErrors.AlreadyActive;

        await clientRepository.ExecuteUpdateAsync(
            x => x.Id == id,
            x => x.SetProperty(p => p.IsActive, true), ct);
        
        await userRepository.ExecuteUpdateAsync(
            x => x.Id == client.AppUserId,
            x => x.SetProperty(p => p.IsActive, true), ct);

        return Result.Success();
    }

    public async Task<Result> DeactivaAsync(Guid id, string userId, CancellationToken ct = default)
    {
        var affectedRows = await clientRepository.ExecuteUpdateAsync(
            x => x.Id == id,
            x => x.SetProperty(p => p.IsActive, false), ct);

        if (affectedRows == 0)
            return ClientErrors.NotFound;

        await userRepository.ExecuteUpdateAsync(
            x => x.Id == userId,
            x => x.SetProperty(p => p.IsActive, false), ct);

        return Result.Success();
    }

}