namespace Application.Features.Users.Commands.Activate;

public sealed record ActivateUserCommand(string UserId) : ICommand;

public sealed class ActivateUserCommandHandler(IUserDomainService userService) : ICommandHandler<ActivateUserCommand>
{
    public async Task<Result> HandleAsync(ActivateUserCommand command, CancellationToken ct = default)
        => await userService.ActivateAsync(command.UserId, ct);
}
