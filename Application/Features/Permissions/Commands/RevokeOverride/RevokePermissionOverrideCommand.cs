namespace Application.Features.Permissions.Commands.RevokeOverride;

public sealed record RevokePermissionOverrideCommand(
    string TargetUserId,
    string Permission,
    string CallerUserId
    ) : ICommand;

internal sealed class RevokePermissionOverrideCommandValidator : AbstractValidator<RevokePermissionOverrideCommand>
{
    public RevokePermissionOverrideCommandValidator()
    {
        RuleFor(x => x.TargetUserId)
            .NotEmpty().WithMessage("Target user ID is required.");

        RuleFor(x => x.Permission)
            .NotEmpty().WithMessage("Permission is required.");

        RuleFor(x => x.CallerUserId)
            .NotEmpty().WithMessage("Caller user ID is required.");
    }
}

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
            return PermissionErrors.PermissionOverrideNotFound;

        logger.LogInformation(
            LogMessages.Permission_Revoked,
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