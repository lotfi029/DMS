namespace Application.Features.Permissions.Commands.AssignToRole;

public sealed record AssignPermissionToRoleCommand(string RoleId, string Permission) : ICommand;

public sealed class AssignPermissionToRoleCommandHandler(
    IPermissionService service
    ) : ICommandHandler<AssignPermissionToRoleCommand>
{
    public async Task<Result> HandleAsync(AssignPermissionToRoleCommand command, CancellationToken ct = default)
        => await service.AssignPermissionToRoleAsync(command.RoleId, command.Permission, ct);
}
