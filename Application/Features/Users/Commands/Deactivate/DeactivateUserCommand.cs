namespace Application.Features.Users.Commands.Deactivate;

public sealed record DeactivateUserCommand(string UserId) : ICommand;

public sealed class DeactivateUserCommandHandler(IUserDomainService userService) : ICommandHandler<DeactivateUserCommand>
{
    public async Task<Result> HandleAsync(DeactivateUserCommand command, CancellationToken ct = default)
        => await userService.DeactivateAsync(command.UserId, ct);
}