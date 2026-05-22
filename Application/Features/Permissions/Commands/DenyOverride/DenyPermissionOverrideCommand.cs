namespace Application.Features.Permissions.Commands.DenyOverride;

public sealed record DenyPermissionOverrideCommand(
    string TargetUserId,
    string Permission,
    string CallerUserId,
    string? Reason = null
    ) : ICommand;

internal sealed class DenyPermissionOverrideCommandHandler(
    IUserPermissionOverrideRepository repo,
    IUnitOfWork unitOfWork,
    IAuditService auditService,
    ILogger<DenyPermissionOverrideCommandHandler> logger
    ) : ICommandHandler<DenyPermissionOverrideCommand>
{
    public async Task<Result> HandleAsync(DenyPermissionOverrideCommand command, CancellationToken ct = default)
    {
        if (!DefaultPermissions.AllDefaultPermissions.Contains(command.Permission))
            return PermissionErrors.InvalidPermission(command.Permission);

        if (await repo.ExistsAsync(p => p.Permission == command.Permission && p.UserId == command.TargetUserId, ct: ct))
            return PermissionErrors.PermissionOverrideAlreadyExists(command.Permission);

        var entity = UserPermissionOverride.Deny(
            userId: command.TargetUserId,
            permission: command.Permission,
            grantedById: command.CallerUserId,
            reason: command.Reason);

        repo.Adapt(entity);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation(
            LogMessages.Permission_OverrideDenied,
            command.Permission, command.TargetUserId, command.CallerUserId);

        await auditService.LogActionAsync(
            action: AuditAction.PermissionDeniedFromUser,
            module: AuditModules.Permissions,
            entityName: AuditEntityNames.PermissionOverride,
            entityId: entity.Id.ToString(),
            description: $"Denied permission '{command.Permission}' from user '{command.TargetUserId}' with override. Reason: {command.Reason}",
            outcome: AuditOutcome.Success,
            ct: ct
            );

        return Result.Success();
    }
}