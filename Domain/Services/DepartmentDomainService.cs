namespace Domain.Services;
// TODO: Refactor this class to use domain events for user-department association changes and handle them in the application layer for better separation of concerns and maintainability.
public class DepartmentDomainService(
    IDepartmentRepository departmentRepository,
    IEmployeeRepository employeeRepository) : IDepartmentDomainService
{
    public async Task<Result<Department>> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        if (await departmentRepository.GetByIdAsync(x => x.Id == id, ct: ct) is not { } department)
            return DepartmentErrors.NotFound;

        return Result.Success(department);
    }

    public async Task<Result<IEnumerable<Department>>> GetAllAsync(CancellationToken ct = default)
    {
        return Result.Success(await departmentRepository.GetAllAsync(ct));
    }

    public async Task<Result<IEnumerable<Employee>>> GetUsersAsync(Expression<Func<Employee, bool>> predicate, CancellationToken ct = default)
    {
        var employees = await employeeRepository.GetAllAsync(predicate, [nameof(Employee.Department)], ct: ct);

        return Result.Success(employees);
    }
    public async Task<Result<Guid>> CreateAsync(string name, string? description, CancellationToken ct = default)
    {
        var entity = Department.Create(name, description);

        if (await departmentRepository.ExistsAsync(e => e.Name == name, ct))
            return DepartmentErrors.DuplicatedName(name);

        departmentRepository.Add(entity, ct);

        return Result.Success(entity.Id);
    }
    public async Task<Result> AddEmployeeAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default)
    {
        if (!await employeeRepository.ExistsAsync(e => e.Id == employeeId, ct))
            return UserErrors.NotFound;

        if (await employeeRepository.ExistsAsync(e => e.Id == employeeId && e.DepartmentId == departmentId, ct: ct))
            return DepartmentErrors.AlreadyInDepartment;

        if (!await departmentRepository.ExistsAsync(e => e.Id == departmentId, ct: ct))
            return DepartmentErrors.NotFound;

        var rowsAffected = await employeeRepository.ExecuteUpdateAsync(
            u => u.Id == employeeId,
            u => u.SetProperty(p => p.DepartmentId, departmentId), 
        ct);

        if (rowsAffected == 0)
            return UserErrors.NotFound;

        return Result.Success();
    }
    public async Task<Result> MoveEmployeeAsync(Guid employeeId, Guid newDepartmentId, CancellationToken ct = default)
    {
        if (!await employeeRepository.ExistsAsync(e => e.Id == employeeId, ct))
            return UserErrors.NotFound;

        if (await employeeRepository.ExistsAsync(e => e.Id == employeeId && e.DepartmentId == newDepartmentId, ct: ct))
            return DepartmentErrors.AlreadyInDepartment;

        if (!await departmentRepository.ExistsAsync(e => e.Id == newDepartmentId, ct: ct))
            return DepartmentErrors.NotFound;

        var rowsAffected = await employeeRepository.ExecuteUpdateAsync(
            u => u.Id == employeeId,
            u => u.SetProperty(p => p.DepartmentId, newDepartmentId), 
        ct);
        
        if (rowsAffected == 0)
            return UserErrors.NotFound;
        
        return Result.Success();
    }
    public async Task<Result> RemoveEmployeeAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default)
    {
        if (!await employeeRepository.ExistsAsync(e => e.Id == employeeId && e.DepartmentId == departmentId, ct))
            return DepartmentErrors.UserNotInDepartment;

        var rowsAffected = await employeeRepository.ExecuteUpdateAsync(
            u => u.Id == employeeId,
            u => u.SetProperty(p => p.DepartmentId, e => null), 
        ct);

        if (rowsAffected == 0)
            return UserErrors.NotFound;

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(Guid id, string name, string? description, CancellationToken ct = default)
    {
        if (await departmentRepository.ExistsAsync(e => e.Name == name && e.Id != id, ct))
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
