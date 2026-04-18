namespace Application.Features.Roles.Commands.Delete;

public sealed record DeleteRoleCommand(string RoleId) : ICommand;

public sealed class DeleteRoleCommandHandler(
    IRoleService service,
    IAuditService auditService,
    ILogger<DeleteRoleCommandHandler> logger) : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> HandleAsync(DeleteRoleCommand command, CancellationToken ct = default)
    {
        var result = await service.DeleteRoleAsync(command.RoleId, ct);

        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.Role_DeleteFailed, command.RoleId, result.Error.Description);
            return result.Error;
        }

        logger.LogInformation(LogMessages.Role_Deleted, command.RoleId);

        await auditService.LogActionAsync(
            action: AuditAction.RoleDeleted,
            module: AuditModules.Roles,
            entityName: AuditEntityNames.Role,
            entityId: command.RoleId,
            description: $"Role '{command.RoleId}' deleted.",
            ct: ct);

        return Result.Success();
    }
}