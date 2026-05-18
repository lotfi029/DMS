namespace Domain.Services;

internal sealed class EmployeeDomainService(
    IEmployeeRepository employeeRepository,
    IUserRepository userRepository) : IEmployeeDomainService
{
    public Result<Guid> Create(
        string userId,
        string jobTitle,
        string? notes = null
        )
    {
        var employee = Employee.Create(
            jobTitle: jobTitle,
            appUserId: userId,
            hireDate: DateOnly.FromDateTime(DateTime.UtcNow),
            notes: notes
        );

        employeeRepository.Add(employee);

        return employee.Id;
    }

    public async Task<Result> UpdateAsync(
        Guid id,
        string? firstName,
        string? lastName,
        string? jobTitle = null,
        string? notes = null,
        CancellationToken ct = default)
    {
        if (await employeeRepository.GetByIdAsync(e => e.Id == id, ct: ct) is not { } employee)
            return EmployeeErrors.NotFound;

        await employeeRepository.ExecuteUpdateAsync(
            x => x.Id == id,
            setter => setter
                .SetProperty(e => e.JobTitle, jobTitle ?? employee.JobTitle)
                .SetProperty(e => e.Notes, notes ?? employee.Notes),
            ct: ct
            );


        await userRepository.ExecuteUpdateAsync(
            x => x.Id == employee.AppUserId,
            setter => setter
                .SetProperty(u => u.FirstName, uu => firstName ?? uu.FirstName)
                .SetProperty(u => u.LastName, uu => lastName ?? uu.LastName),
            ct: ct
            );

        return Result.Success();
    }

    public async Task<Result> DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        if (await employeeRepository.GetByIdAsync(e => e.Id == id, ct: ct) is not { } employee)
            return EmployeeErrors.NotFound;

        if (!employee.IsActive)
            return EmployeeErrors.AlreadyInactive;

        await employeeRepository.ExecuteUpdateAsync(
            x => x.Id == id,
            setter => setter.SetProperty(e => e.IsActive, false),
            ct: ct
            );

        await userRepository.ExecuteUpdateAsync(
            x => x.Id == employee.AppUserId,
            setter => setter.SetProperty(u => u.IsActive, false),
            ct: ct
            );

        return Result.Success();
    }

    public async Task<Result> ActivateAsync(Guid id, CancellationToken ct = default)
    {
        if (await employeeRepository.GetByIdAsync(e => e.Id == id, ct: ct) is not { } employee)
            return EmployeeErrors.NotFound;

        if (employee.IsActive)
            return EmployeeErrors.AlreadyActive;

        await employeeRepository.ExecuteUpdateAsync(
            x => x.Id == id,
            setter => setter.SetProperty(e => e.IsActive, true),
            ct: ct
            );
        await userRepository.ExecuteUpdateAsync(
            x => x.Id == employee.AppUserId,
            setter => setter.SetProperty(u => u.IsActive, true),
            ct: ct
            );
        return Result.Success();
    }
}

