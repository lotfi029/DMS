namespace Application.Features.Departments.Commands.MoveUser;

public sealed record MoveEmployeeToDepartmentCommand(Guid UserId, Guid ToDepartmentId) : ICommand;

internal sealed class MoveEmployeeToDepartmentCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    ILogger<MoveEmployeeToDepartmentCommandHandler> logger) : ICommandHandler<MoveEmployeeToDepartmentCommand>
{
    public async Task<Result> HandleAsync(MoveEmployeeToDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.MoveEmployeeAsync(command.UserId, command.ToDepartmentId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Dept_UserMoved, command.UserId, command.ToDepartmentId);

        await auditService.LogActionAsync(
            action: AuditAction.UserMovedBetweenDepartments,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.ToDepartmentId.ToString(),
            description: $"User '{command.UserId}' moved to department '{command.ToDepartmentId}'.",
            ct: ct);

        return Result.Success();
    }
}