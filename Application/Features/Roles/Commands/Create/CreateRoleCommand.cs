namespace Application.Features.Roles.Commands.Create;

public sealed record CreateRoleCommand(string RoleName) : ICommand;

public sealed class CreateRoleCommandHandler(IRoleService service) : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> HandleAsync(CreateRoleCommand command, CancellationToken ct = default)
        => await service.CreateRoleAsync(command.RoleName, ct);
}
