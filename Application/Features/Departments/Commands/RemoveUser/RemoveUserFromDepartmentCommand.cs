namespace Application.Features.Departments.Commands.RemoveUser;

public sealed record RemoveUserFromDepartmentCommand(string UserId, Guid DepartmentId) : ICommand;

internal sealed class RemoveUserCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    ILogger<RemoveUserCommandHandler> logger) : ICommandHandler<RemoveUserFromDepartmentCommand>
{
    public async Task<Result> HandleAsync(RemoveUserFromDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.RemoveUserAsync(command.UserId, command.DepartmentId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Dept_UserRemoved, command.UserId, command.DepartmentId);

        await auditService.LogActionAsync(
            action: AuditAction.UserRemovedFromDepartment,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.DepartmentId.ToString(),
            description: $"User '{command.UserId}' removed from department '{command.DepartmentId}'.",
            ct: ct);

        return Result.Success();
    }
}
