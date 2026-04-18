namespace Application.Features.Roles.Commands.Update;

public sealed record UpdateRoleCommand(string RoleId, string NewRoleName) : ICommand;

public sealed class UpdateRoleCommandHandler(
    IRoleService service,
    IAuditService auditService,
    ILogger<UpdateRoleCommandHandler> logger) : ICommandHandler<UpdateRoleCommand>
{
    public async Task<Result> HandleAsync(UpdateRoleCommand command, CancellationToken ct = default)
    {
        var result = await service.UpdateRoleAsync(command.RoleId, command.NewRoleName, ct);

        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.Role_UpdateFailed, command.RoleId, result.Error.Description);
            return result.Error;
        }

        logger.LogInformation(LogMessages.Role_Updated, command.RoleId, command.NewRoleName);

        await auditService.LogActionAsync(
            action: AuditAction.RoleUpdated,
            module: AuditModules.Roles,
            entityName: AuditEntityNames.Role,
            entityId: command.RoleId,
            description: $"Role '{command.RoleId}' renamed to '{command.NewRoleName}'.",
            ct: ct);

        return Result.Success();
    }
}