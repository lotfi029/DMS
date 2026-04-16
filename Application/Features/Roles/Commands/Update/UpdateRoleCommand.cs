namespace Application.Features.Roles.Commands.Update;

public sealed record UpdateRoleCommand(string RoleId, string NewRoleName) : ICommand;

public sealed class UpdateRoleCommandHandler(IRoleService service) : ICommandHandler<UpdateRoleCommand>
{
    public async Task<Result> HandleAsync(UpdateRoleCommand command, CancellationToken ct = default)
        => await service.UpdateRoleAsync(command.RoleId, command.NewRoleName, ct);
}
