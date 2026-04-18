namespace Application.Features.Departments.Commands.MoveUser;

public sealed record MoveUserToDepartmentCommand(string UserId, Guid ToDepartmentId) : ICommand;

internal sealed class MoveUserCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    ILogger<MoveUserCommandHandler> logger) : ICommandHandler<MoveUserToDepartmentCommand>
{
    public async Task<Result> HandleAsync(MoveUserToDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.MoveUserAsync(command.UserId, command.ToDepartmentId, ct);

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