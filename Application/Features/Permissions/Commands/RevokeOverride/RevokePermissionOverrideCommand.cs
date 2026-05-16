namespace Application.Features.Permissions.Commands.RevokeOverride;

public sealed record RevokePermissionOverrideCommand(
    string TargetUserId,
    string Permission,
    string CallerUserId
    ) : ICommand;

internal sealed class RevokePermissionOverrideCommandHandler(
    IUserPermissionOverrideRepository repo,
    IAuditService auditService,
    ILogger<RevokePermissionOverrideCommandHandler> logger)
    : ICommandHandler<RevokePermissionOverrideCommand>
{
    public async Task<Result> HandleAsync(RevokePermissionOverrideCommand command, CancellationToken ct = default)
    {
        var rowsDeleted = await repo.ExecuteDeleteAsync(
            o => o.UserId == command.TargetUserId
                && o.Permission == command.Permission, ct);

        if (rowsDeleted == 0)
            return Error.NotFound(
                "PermissionOverrideNotFound", 
                $"No permission override found for user {command.TargetUserId} and permission {command.Permission}.");

        logger.LogInformation(
            "Revoked permission override for user {TargetUserId} and permission {Permission}.", 
            command.TargetUserId,
            command.Permission);

        await auditService.LogActionAsync(
            action: AuditAction.PermissionRemovedFromRole,
            module: AuditModules.Permissions,
            entityName: AuditEntityNames.User,
            entityId: command.TargetUserId,
            description: $"REVOKE override: '{command.Permission}' removed from user '{command.TargetUserId}'.",
            ct: ct);

        return Result.Success();
    }
}