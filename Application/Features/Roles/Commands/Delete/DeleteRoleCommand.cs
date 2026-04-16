namespace Application.Features.Roles.Commands.Delete;

public sealed record DeleteRoleCommand(string RoleId) : ICommand;

public sealed class DeleteRoleCommandHandler(IRoleService service) : ICommandHandler<DeleteRoleCommand>
{
    public async Task<Result> HandleAsync(DeleteRoleCommand command, CancellationToken ct = default)
        => await service.DeleteRoleAsync(command.RoleId, ct);
}
