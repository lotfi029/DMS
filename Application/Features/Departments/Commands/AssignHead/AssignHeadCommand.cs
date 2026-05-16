namespace Application.Features.Departments.Commands.AssignHead;

public sealed record AssignDepartmentHeadCommand(
    Guid DepartmentId,
    Guid EmployeeId
    ) : ICommand;

internal sealed class AssignDepartmentHeadCommandHandler(
    IUnitOfWork unitOfWork,
    IRoleService roleService,
    IEmployeeRepository employeeRepository,
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService) : ICommandHandler<AssignDepartmentHeadCommand>
{
    public async Task<Result> HandleAsync(AssignDepartmentHeadCommand command, CancellationToken ct = default)
    {
        if (await employeeRepository.GetByIdAsync(command.EmployeeId, ct) is not Employee employee)
            return EmployeeErrors.NotFound;

        if (await roleService.UserInRoleAsync(employee.AppUserId, DefaultRoles.DepartmentHead.Id, ct) is { IsFailure: true } roleError)
            return roleError;

        if (await departmentDomainService.AssignDepartmentHeadAsync(command.EmployeeId, command.DepartmentId, ct) is { IsFailure: true } domainError)
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