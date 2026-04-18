namespace Application.Features.Roles.Commands.RemoveFromUser;

public sealed record RemoveRoleFromUserCommand(string UserId, string RoleId) : ICommand;

public sealed class RemoveRoleFromUserCommandHandler(
    IRoleService service,
    IAuditService auditService,
    ILogger<RemoveRoleFromUserCommandHandler> logger) : ICommandHandler<RemoveRoleFromUserCommand>
{
    public async Task<Result> HandleAsync(RemoveRoleFromUserCommand command, CancellationToken ct = default)
    {
        var result = await service.RemoveRoleFromUserAsync(command.UserId, command.RoleId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Role_RemovedFromUser, command.RoleId, command.UserId);

        await auditService.LogActionAsync(
            action: AuditAction.RoleRemovedFromUser,
            module: AuditModules.Roles,
            entityName: AuditEntityNames.User,
            entityId: command.UserId,
            description: $"Role '{command.RoleId}' removed from user '{command.UserId}'.",
            ct: ct);

        return Result.Success();
    }
}
