namespace Application.Features.Users.Commands.Delete;

public sealed record DeleteUserCommand(string UserId) : ICommand;
// TODO: add soft delete and remove from user service
public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    public Task<Result> HandleAsync(DeleteUserCommand command, CancellationToken ct = default)
        => throw new NotImplementedException();
}
