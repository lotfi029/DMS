namespace Domain.Services;

internal sealed class EmployeeDomainService(
    IEmployeeRepository employeeRepository,
    IUserRepository userRepository) : IEmployeeDomainService
{
    public Result<Guid> Create(
        string userId,
        string jobTitle,
        ContractType contractType,
        string? phoneNumber = null,
        string? emergencyContactName = null,
        string? emergencyContactPhone = null,
        string? notes = null
        )
    {
        var employee = Employee.Create(
            jobTitle: jobTitle,
            appUserId: userId,
            hireDate: DateOnly.FromDateTime(DateTime.UtcNow),
            contractType: contractType,
            phoneNumber: phoneNumber,
            emergencyName: emergencyContactName,
            emergencyPhone: emergencyContactPhone,

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
        ContractType? contractType = null,
        string? phoneNumber = null,
        string? emergencyContactName = null,
        string? emergencyContactPhone = null,
        string? notes = null,
        CancellationToken ct = default)
    {
        if (await employeeRepository.GetByIdAsync(e => e.Id == id, ct: ct) is not { } employee)
            return EmployeeErrors.NotFound;

        await employeeRepository.ExecuteUpdateAsync(
            x => x.Id == id,
            setter =>
            {
                if (jobTitle is not null)
                    setter.SetProperty(e => e.JobTitle, jobTitle);
                if (contractType is not null)
                    setter.SetProperty(e => e.ContractType, contractType.Value);
                if (phoneNumber is not null)
                    setter.SetProperty(e => e.PhoneNumber, phoneNumber);
                if (emergencyContactName is not null)
                    setter.SetProperty(e => e.EmergencyContactName, emergencyContactName);
                if (emergencyContactPhone is not null)
                    setter.SetProperty(e => e.EmergencyContactPhone, emergencyContactPhone);
                if (notes is not null)
                    setter.SetProperty(e => e.Notes, notes);
            },
            ct: ct
            );


        await userRepository.ExecuteUpdateAsync(
            x => x.Id == employee.AppUserId,
            setter => 
            { 
                if (firstName is not null)
                    setter.SetProperty(u => u.FirstName, firstName);
                if (lastName is not null)
                    setter.SetProperty(u => u.LastName, lastName);
            },
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

