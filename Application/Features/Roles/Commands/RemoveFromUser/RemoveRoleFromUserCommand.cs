namespace Application.Features.Roles.Commands.RemoveFromUser;

public sealed record RemoveRoleFromUserCommand(string UserId, string RoleId) : ICommand;

public sealed class RemoveRoleFromUserCommandHandler(IRoleService service) : ICommandHandler<RemoveRoleFromUserCommand>
{
    public async Task<Result> HandleAsync(RemoveRoleFromUserCommand command, CancellationToken ct = default)
        => await service.RemoveRoleFromUserAsync(command.UserId, command.RoleId, ct);
}
