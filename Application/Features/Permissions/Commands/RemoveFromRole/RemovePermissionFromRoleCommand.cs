namespace Application.Features.Permissions.Commands.RemoveFromRole;

public sealed record RemovePermissionFromRoleCommand(string RoleId, string Permission) : ICommand;

public sealed class RemovePermissionFromRoleCommandHandler(IPermissionService service) : ICommandHandler<RemovePermissionFromRoleCommand>
{
    public async Task<Result> HandleAsync(RemovePermissionFromRoleCommand command, CancellationToken ct = default)
        => await service.RemovePermissionFromRoleAsync(command.RoleId, command.Permission, ct);
}
