namespace Application.Features.Departments.Commands.AssignHead;

public sealed record AssignDepartmentHeadCommand(Guid DepartmentId, Guid EmployeeId) : ICommand;

internal sealed class AssignDepartmentHeadCommandValidator : AbstractValidator<AssignDepartmentHeadCommand>
{
    public AssignDepartmentHeadCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department ID is required.");
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");
    }
}

internal sealed class AssignDepartmentHeadCommandHandler(
    IUnitOfWork unitOfWork,
    IRoleService roleService,
    IEmployeeRepository employeeRepository,
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService) : ICommandHandler<AssignDepartmentHeadCommand>
{
    public async Task<Result> HandleAsync(AssignDepartmentHeadCommand command, CancellationToken ct = default)
    {
        if (await employeeRepository.GetByIdAsync(x => x.AppUserId == command.EmployeeId.ToString(), ct: ct) is not { } employee)
            return EmployeeErrors.NotFound;

        if (await roleService.UserInRoleAsync(employee.AppUserId, DefaultRoles.DepartmentHead.Name!, ct) is { IsFailure: true } roleError)
            return roleError;

        if (await departmentDomainService.AssignDepartmentHeadAsync(employee.Id, command.DepartmentId, ct) is { IsFailure: true } domainError)
            return domainError;

        await unitOfWork.SaveChangesAsync(ct);

        await auditService.LogActionAsync(
            action: AuditAction.AssignedDepartmentHead,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.DepartmentId.ToString(),
            outcome: AuditOutcome.Success,
            ct: ct);

        return Result.Success();


    }
}