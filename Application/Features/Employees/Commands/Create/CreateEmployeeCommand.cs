namespace Application.Features.Employees.Commands.Create;

public sealed record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string JobTitle,
    string? RoleId,
    Guid? DepartmentId,
    IEnumerable<string> GrantPermissions,
    IEnumerable<string> DenyPermissions,
    string? Notes) : ICommand<Guid>;

internal sealed class CreateEmployeeCommandHandler(
    IEmployeeDomainService employeeService,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IAuditService auditService,
    IDepartmentRepository departmentRepository,
    IUserPermissionOverrideRepository userPermission,
    IEmployeeDepartmentRepository employeeDepartmentRepository,
    ILogger<CreateEmployeeCommandHandler> logger) : ICommandHandler<CreateEmployeeCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CreateEmployeeCommand command, CancellationToken ct = default)
    {
        if (!await departmentRepository.ExistsAsync(x => x.Id == command.DepartmentId, ct))
            return DepartmentErrors.NotFound;

        var transaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var registerRequest = new RegisterRequest(
                command.FirstName, command.LastName,
                command.Password, command.Email, command.UserName);

            var registerResult = await authService.RegisterAsync(
                command.RoleId!, UserType.Employee, registerRequest, ct);

            if (registerResult.IsFailure)
            {
                await transaction.RollbackAsync(ct);
                return registerResult.Error;
            }

            var employeeResult = employeeService.Create(
                userId: registerResult.Value!,
                jobTitle: command.JobTitle,
                notes: command.Notes
                );

            if (employeeResult.IsFailure)
            {
                await transaction.RollbackAsync(ct);
                return employeeResult.Error;
            }

            var employeeDepartment = EmployeeDepartment.Create(
                employeeId: employeeResult.Value,
                departmentId: command.DepartmentId!.Value);

            employeeDepartmentRepository.Add(employeeDepartment);
            await unitOfWork.SaveChangesAsync(ct);

            if (command.GrantPermissions.Any() || command.DenyPermissions.Any())
            {
                var permissionResult = await AddPermissions(
                    command.GrantPermissions,
                    command.DenyPermissions,
                    registerResult.Value!,
                    ct);
                if (permissionResult.IsFailure)
                {
                    await transaction.RollbackAsync(ct);
                    return permissionResult.Error;
                }
            }

            await auditService.LogActionAsync(
                AuditAction.EmployeeCreated,
                module: AuditModules.Employees,
                entityName: AuditEntityNames.Employee,
                entityId: employeeResult.ToString(),
                outcome: AuditOutcome.Success,
                ct: ct);

            await transaction.CommitAsync(ct);

            logger.LogInformation(
                "Employee created: userId={UserId}, department={DeptId}, role={RoleId}",
                registerResult.Value!, command.DepartmentId, command.RoleId ?? "Employee (default)");

            return employeeResult;
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
    private async Task<Result> AddPermissions(
        IEnumerable<string> grantPermissions,
        IEnumerable<string> denyPermission,
        string userId, CancellationToken ct)
    {

        foreach (var permission in grantPermissions)
        {
            if (!DefaultPermissions.AllDefaultPermissions.Contains(permission))
                continue;

            var grant = UserPermissionOverride.Grant(
                userId,
                permission,
                "system");

            userPermission.Add(grant);
        }
        foreach (var permission in denyPermission)
        {
            var deny = UserPermissionOverride.Deny(
                userId,
                permission,
                "system");

            userPermission.Add(deny);
        }

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}