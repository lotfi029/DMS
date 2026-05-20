namespace Application.Features.EmployeeDepartments.Commands.MoveAll;

public sealed record MoveAllEmployeeCommand(Guid FromDepartmentId, Guid ToDepartmentId) : ICommand;

internal sealed class MoveAllEmployeeCommandValidator : AbstractValidator<MoveAllEmployeeCommand>
{
    public MoveAllEmployeeCommandValidator()
    {
        RuleFor(x => x.FromDepartmentId)
            .NotEmpty().WithMessage("FromDepartmentId is required.");
        RuleFor(x => x.ToDepartmentId)
            .NotEmpty().WithMessage("ToDepartmentId is required.")
            .NotEqual(x => x.FromDepartmentId).WithMessage("ToDepartmentId must be different from FromDepartmentId.");
    }
}

internal sealed class MoveAllEmployeeCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    ILogger<MoveAllEmployeeCommandHandler> logger) : ICommandHandler<MoveAllEmployeeCommand>
{
    public async Task<Result> HandleAsync(MoveAllEmployeeCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService
            .MoveAllEmployeeAsync(command.FromDepartmentId, command.ToDepartmentId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Dept_AllUsersMoved, command.FromDepartmentId, command.ToDepartmentId);
        
        await auditService.LogActionAsync(
            action: AuditAction.AllUsersMovedBetweenDepartments,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.ToDepartmentId.ToString(),
            description: $"All users moved from department '{command.FromDepartmentId}' to department '{command.ToDepartmentId}'.",
            ct: ct);

        return Result.Success();
    }
}