namespace Application.Features.Permissions.Commands.AssignToRole;

public sealed record AssignPermissionToRoleCommand(string RoleId, string Permission) : ICommand;

public sealed class AssignPermissionToRoleCommandHandler(
    IPermissionService service,
    IAuditService auditService
    ) : ICommandHandler<AssignPermissionToRoleCommand>
{
    public async Task<Result> HandleAsync(AssignPermissionToRoleCommand command, CancellationToken ct = default)
    {
        var result = await service.AssignPermissionToRoleAsync(command.RoleId, command.Permission, ct);

        if (result.IsFailure)
            return result.Error;

        await auditService.LogActionAsync(
            action: AuditAction.PermissionAssignedToRole,
            module: AuditModules.Permissions,
            entityName: AuditEntityNames.RoleClaim,
            ct: ct);

        return Result.Success();
    }
}