namespace Application.Features.Roles.Commands.AssignToUser;

public sealed record AssignRoleToUserCommand(string UserId, string RoleId) : ICommand;

public sealed class AssignRoleToUserCommandHandler(IRoleService service) : ICommandHandler<AssignRoleToUserCommand>
{
    public async Task<Result> HandleAsync(AssignRoleToUserCommand command, CancellationToken ct = default)
        => await service.AssignRoleToUserAsync(command.UserId, command.RoleId, ct);
}
