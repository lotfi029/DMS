namespace Application.Features.Permissions.Commands.GrantOverride;

public sealed record GrantPermissionOverrideCommand(
    string TargetUserId,
    string Permission,
    string CallerUserId,
    string? Reason = null,
    DateTime? ExpiresAt = null
    ) : ICommand;

internal sealed class GrantPermissionOverrideCommandHandler(
    IUserPermissionOverrideRepository repo,
    IUnitOfWork unitOfWork,
    IAuditService auditService,
    ILogger<GrantPermissionOverrideCommandHandler> logger
    ) : ICommandHandler<GrantPermissionOverrideCommand>
{
    public async Task<Result> HandleAsync(GrantPermissionOverrideCommand command, CancellationToken ct = default)
    {
        if (!DefaultPermissions.AllDefaultPermissions.Contains(command.Permission))
            return Error.BadRequest(
                "InvalidPermission", 
                $"The permission '{command.Permission}' is not a valid permission.");

        if (await repo.ExistsAsync(p => p.Permission == command.Permission && p.UserId == command.TargetUserId, ct))
            return Error.Conflict(
                "PermissionAlreadyExists",
                $"The permission '{command.Permission}' already exists for user '{command.TargetUserId}'.");

        var entity = UserPermissionOverride.Grant(
            command.TargetUserId,
            command.Permission,
            command.CallerUserId,
            command.Reason,
            command.ExpiresAt);

        repo.Add(entity);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation(
           "Permission override GRANTED: {Permission} → user {UserId} by {CallerId}",
           command.Permission, command.TargetUserId, command.CallerUserId);

        await auditService.LogActionAsync(
            action: AuditAction.PermissionGrantedToUser,
            module: AuditModules.Permissions,
            entityName: AuditEntityNames.PermissionOverride,
            entityId: entity.Id.ToString(),
            description: $"Granted permission '{command.Permission}' to user '{command.TargetUserId}' with override. Reason: {command.Reason}",
            outcome: AuditOutcome.Success,
            ct: ct
            );

        return Result.Success();
    }
}