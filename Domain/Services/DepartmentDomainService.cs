namespace Domain.Services;
// TODO: Refactor this class to use domain events for user-department association changes and handle them in the application layer for better separation of concerns and maintainability.
public class DepartmentDomainService(
    IDepartmentRepository departmentRepository,
    IUserRepository userRepository) : IDepartmentDomainService
{
    public async Task<Result<Department>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (await departmentRepository.GetByIdAsync(x => x.Id == id, ct) is not { } department)
            return DepartmentErrors.NotFound;

        return Result.Success(department);
    }

    public async Task<Result<IEnumerable<Department>>> GetAllAsync(CancellationToken ct = default)
    {
        return Result.Success(await departmentRepository.GetAllAsync(ct));
    }

    public async Task<Result<IEnumerable<ApplicationUser>>> GetUsersAsync(Expression<Func<ApplicationUser, bool>> predicate, CancellationToken ct = default)
    {
        return Result.Success(await userRepository.GetAllAsync(predicate, ct));
    }
    public async Task<Result> CreateAsync(string name, string? description, CancellationToken ct = default)
    {
        var entity = Department.Create(name, description);

        if (await departmentRepository.ExistsAsync(e => e.Name == name, ct))
            return DepartmentErrors.DuplicatedName(name);

        departmentRepository.Add(entity, ct);

        return Result.Success();
    }
    public async Task<Result> AddUserAsync(string userId, Guid departmentId, CancellationToken ct = default)
    {
        if (await userRepository.ExistsAsync(e => e.Id == userId && e.IsActive, ct))
            return UserErrors.NotFound;

        if (await userRepository.ExistsAsync(e => e.DepartmentId == departmentId, ct))
            return DepartmentErrors.AlreadyInDepartment;

        if (await departmentRepository.ExistsAsync(e => e.Id == departmentId, ct))
            return DepartmentErrors.NotFound;

        var rowsAffected = await userRepository.ExecuteUpdateAsync(
            u => u.Id == userId,
            u => u.SetProperty(p => p.DepartmentId, departmentId), ct);

        if (rowsAffected == 0)
            return UserErrors.NotFound;

        return Result.Success();
    }
    public async Task<Result> MoveUserAsync(string userId, Guid newDepartmentId, CancellationToken ct = default)
    {
        if (!await userRepository.ExistsAsync(e => e.Id == userId && e.IsActive, ct))
            return UserErrors.NotFound;
        if (await userRepository.ExistsAsync(e => e.DepartmentId == newDepartmentId, ct))
            return DepartmentErrors.AlreadyInDepartment;
        if (await departmentRepository.ExistsAsync(e => e.Id == newDepartmentId, ct))
            return DepartmentErrors.NotFound;

        var rowsAffected = await userRepository.ExecuteUpdateAsync(
            u => u.Id == userId,
            u => u.SetProperty(p => p.DepartmentId, newDepartmentId), ct);
        
        if (rowsAffected == 0)
            return UserErrors.NotFound;
        
        return Result.Success();
    }
    public async Task<Result> RemoveUserAsync(string userId, Guid departmentId, CancellationToken ct = default)
    {
        if (!await userRepository.ExistsAsync(e => e.DepartmentId == departmentId, ct))
            return DepartmentErrors.UserNotInDepartment;

        var rowsAffected = await userRepository.ExecuteUpdateAsync(
            u => u.Id == userId,
            u => u.SetProperty(p => p.DepartmentId, e => null), ct);

        if (rowsAffected == 0)
            return UserErrors.NotFound;

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(Guid id, string name, string? description, CancellationToken ct = default)
    {
        if (await departmentRepository.ExistsAsync(e => e.Name == name, ct))
            return DepartmentErrors.DuplicatedName(name);

        var affectedRows = await departmentRepository.ExecuteUpdateAsync(
            d => d.Id == id,
            d =>
            {
                d.SetProperty(p => p.Name, name);

                if (!string.IsNullOrWhiteSpace(description))
                    d.SetProperty(p => p.Description, description);
            }
            , ct);

        if (affectedRows == 0)
            return DepartmentErrors.NotFound;

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var affectedRows = await departmentRepository
            .ExecuteDeleteAsync(d => d.Id == id, ct);

        if (affectedRows == 0)
            return DepartmentErrors.NotFound;

        return Result.Success();
    }
}
