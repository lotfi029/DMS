namespace Application.Features.EmployeeDepartments.Commands.RemoveUser;

public sealed record RemoveEmployeeFromDepartmentCommand(
    Guid DepartmentId,
    Guid EmployeeId
    ) : ICommand;

internal sealed class RemoveEmployeeFromDepartmentCommandValidator : AbstractValidator<RemoveEmployeeFromDepartmentCommand>
{
    public RemoveEmployeeFromDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department ID is required.");
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");
    }
}


internal sealed class RemoveEmployeeFromDepartmentCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    ILogger<RemoveEmployeeFromDepartmentCommandHandler> logger) : ICommandHandler<RemoveEmployeeFromDepartmentCommand>
{
    public async Task<Result> HandleAsync(RemoveEmployeeFromDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.RemoveEmployeeAsync(command.EmployeeId, command.DepartmentId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Dept_UserRemoved, command.EmployeeId, command.DepartmentId);

        await auditService.LogActionAsync(
            action: AuditAction.UserRemovedFromDepartment,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.DepartmentId.ToString(),
            description: $"User '{command.EmployeeId}' removed from department '{command.DepartmentId}'.",
            ct: ct);

        return Result.Success();
    }
}
