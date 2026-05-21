namespace Application.Features.EmployeeDepartments.Commands.MoveUser;

public sealed record MoveEmployeeToDepartmentCommand(
    Guid DepartmentId,
    Guid EmployeeId
    ) : ICommand;

internal sealed class MoveEmployeeToDepartmentCommandValidator : AbstractValidator<MoveEmployeeToDepartmentCommand>
{
    public MoveEmployeeToDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department ID is required.");
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");
    }
}


internal sealed class MoveEmployeeToDepartmentCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    ILogger<MoveEmployeeToDepartmentCommandHandler> logger) : ICommandHandler<MoveEmployeeToDepartmentCommand>
{
    public async Task<Result> HandleAsync(MoveEmployeeToDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.MoveEmployeeAsync(command.EmployeeId, command.DepartmentId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Dept_UserMoved, command.EmployeeId, command.DepartmentId);

        await auditService.LogActionAsync(
            action: AuditAction.UserMovedBetweenDepartments,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.DepartmentId.ToString(),
            description: $"User '{command.EmployeeId}' moved to department '{command.DepartmentId}'.",
            ct: ct);

        return Result.Success();
    }
}