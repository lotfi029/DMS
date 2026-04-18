namespace Application.Features.Permissions.Commands.RemoveFromRole;

public sealed record RemovePermissionFromRoleCommand(string RoleId, string Permission) : ICommand;

public sealed class RemovePermissionFromRoleCommandHandler(
    IPermissionService service,
    IAuditService auditService) : ICommandHandler<RemovePermissionFromRoleCommand>
{
    public async Task<Result> HandleAsync(RemovePermissionFromRoleCommand command, CancellationToken ct = default)
    {
        var result = await service.RemovePermissionFromRoleAsync(command.RoleId, command.Permission, ct);

        if (result.IsFailure)
            return result.Error;

        await auditService.LogActionAsync(
            action: AuditAction.PermissionRemovedFromRole,
            module: AuditModules.Permissions,
            entityName: AuditEntityNames.RoleClaim,
            ct: ct);

        return Result.Success();
    }
}
