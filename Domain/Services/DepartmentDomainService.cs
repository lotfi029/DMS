namespace Domain.Services;
// TODO: Refactor this class to use domain events for user-department association changes and handle them in the application layer for better separation of concerns and maintainability.
public class DepartmentDomainService(
    IDepartmentRepository departmentRepository,
    IEmployeeRepository employeeRepository,
    IEmployeeDepartmentRepository employeeDepartmentRepository) : IDepartmentDomainService
{
    public async Task<Result<Guid>> CreateAsync(string name, string? description, CancellationToken ct = default)
    {
        var entity = Department.Create(name, description);

        if (await departmentRepository.ExistsAsync(e => e.Name == name, ct))
            return DepartmentErrors.DuplicatedName(name);

        departmentRepository.Add(entity);

        return Result.Success(entity.Id);
    }
    public async Task<Result> AssignDepartmentHeadAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default)
    {
        if (!await departmentRepository.ExistsAsync(e => e.Id == departmentId, ct: ct))
            return DepartmentErrors.NotFound;

        if (!await employeeDepartmentRepository.ExistsAsync(ed => ed.EmployeeId == employeeId && ed.DepartmentId == departmentId, ct: ct))
            employeeDepartmentRepository.Add(EmployeeDepartment.Create(employeeId, departmentId));

        await departmentRepository.ExecuteUpdateAsync(
            d => d.Id == departmentId,
            d => d.SetProperty(p => p.DepartmentHeadId, employeeId),
            ct);

        return Result.Success();
    }
    public async Task<Result> AddEmployeeAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default)
    {
        if (!await employeeRepository.ExistsAsync(e => e.Id == employeeId, ct))
            return UserErrors.NotFound;

        if (!await departmentRepository.ExistsAsync(e => e.Id == departmentId, ct: ct))
            return DepartmentErrors.NotFound;

        if (await employeeDepartmentRepository.ExistsAsync(ed => ed.EmployeeId == employeeId && ed.DepartmentId == departmentId, ct: ct))
            return DepartmentErrors.AlreadyInDepartment;


        employeeDepartmentRepository.Add(EmployeeDepartment.Create(employeeId, departmentId));

        return Result.Success();
    }
    public async Task<Result> MoveEmployeeAsync(Guid employeeId, Guid newDepartmentId, CancellationToken ct = default)
    {
        if (await employeeDepartmentRepository.ExistsAsync(ed => ed.EmployeeId == employeeId && ed.DepartmentId == newDepartmentId, ct: ct))
            return DepartmentErrors.AlreadyInDepartment;

        if (!await employeeRepository.ExistsAsync(e => e.Id == employeeId, ct))
            return UserErrors.NotFound;

        if (!await departmentRepository.ExistsAsync(e => e.Id == newDepartmentId, ct: ct))
            return DepartmentErrors.NotFound;

        var rowsAffected = await employeeDepartmentRepository.ExecuteUpdateAsync(
            u => u.EmployeeId == employeeId,
            u => u.SetProperty(p => p.DepartmentId, newDepartmentId),
        ct);

        if (rowsAffected == 0)
            return UserErrors.NotFound;

        return Result.Success();
    }
    public async Task<Result> RemoveEmployeeAsync(Guid employeeId, Guid departmentId, CancellationToken ct = default)
    {
        if (!await employeeDepartmentRepository.ExistsAsync(e => e.EmployeeId == employeeId && e.DepartmentId == departmentId, ct))
            return DepartmentErrors.UserNotInDepartment;

        var rowsAffected = await employeeDepartmentRepository.ExecuteUpdateAsync(
            u => u.EmployeeId == employeeId && u.DepartmentId == departmentId,
            u => u.SetProperty(p => p.IsActive, false),
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
