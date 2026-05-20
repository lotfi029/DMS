using Application.DTOs.Employees;

namespace Application.Features.Employees.Commands.Create;

public sealed record CreateEmployeeCommand(
    CreateEmployeeRequest Request) : ICommand<Guid>;
internal sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.Request.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Request.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Request.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Request.PhoneNumber)
            .NotEmpty()
            .MaximumLength(20)
            .When(x => x.Request.PhoneNumber is not null);
        RuleFor(x => x.Request.UserName)
            .NotEmpty()
            .MaximumLength(256);
        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .MinimumLength(8);
        RuleFor(x => x.Request.JobTitle)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.Request.ContractType)
            .IsInEnum();
        RuleFor(x => x.Request.DenyPermissions)
            .ForEach(permission => permission.MaximumLength(100));
        
        RuleFor(x => x.Request.GrantPermissions)
            .ForEach(permission => permission.MaximumLength(100));
        
        RuleFor(x => x.Request.EmergencyContactName)
            .NotEmpty()
            .When(x => x.Request.EmergencyContactName is not null);
        
        RuleFor(x => x.Request.EmergencyContactPhone)
            .NotEmpty()
            .MaximumLength(20)
            .When(x => x.Request.EmergencyContactPhone is not null);
        RuleFor(x => x.Request.DepartmentId)
            .NotEmpty()
            .When(x => x.Request.DepartmentId is not null);
        RuleFor(x => x.Request.Notes)
            .MaximumLength(2000)
            .When(x => x.Request.Notes is not null);
    }
}
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
        if (!await departmentRepository.ExistsAsync(x => x.Id == command.Request.DepartmentId, ct: ct))
            return DepartmentErrors.NotFound;

        var transaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var registerRequest = new RegisterRequest(
                FirstName: command.Request.FirstName, 
                LastName: command.Request.LastName,
                Password: command.Request.Password, 
                Email: command.Request.Email,
                UserName: command.Request.UserName, 
                PhoneNumber: command.Request.PhoneNumber ?? string.Empty);

            var registerResult = await authService.RegisterAsync(
                command.Request.RoleId!, UserType.Employee, registerRequest, ct);

            if (registerResult.IsFailure)
            {
                await transaction.RollbackAsync(ct);
                return registerResult.Error;
            }

            var employeeResult = employeeService.Create(
                userId: registerResult.Value!,
                jobTitle: command.Request.JobTitle,
                contractType: command.Request.ContractType,
                emergencyContactName: command.Request.EmergencyContactName,
                emergencyContactPhone: command.Request.EmergencyContactPhone,
                notes: command.Request.Notes
                );

            if (employeeResult.IsFailure)
            {
                await transaction.RollbackAsync(ct);
                return employeeResult.Error;
            }

            var employeeDepartment = EmployeeDepartment.Create(
                employeeId: employeeResult.Value,
                departmentId: command.Request.DepartmentId!.Value);

            employeeDepartmentRepository.Add(employeeDepartment);
            await unitOfWork.SaveChangesAsync(ct);

            if (command.Request.GrantPermissions.Any() || command.Request.DenyPermissions.Any())
            {
                var permissionResult = await AddPermissions(
                    command.Request.GrantPermissions,
                    command.Request.DenyPermissions,
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
                registerResult.Value!, command.Request.DepartmentId, command.Request.RoleId ?? "Employee (default)");

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