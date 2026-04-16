namespace Application.Features.Users.Commands.Update;

public sealed record UpdateUserCommand(string UserId, UpdateUserRequest Request) : ICommand;

public sealed class UpdateUserCommandHandler(
    IUserDomainService userService) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> HandleAsync(UpdateUserCommand command, CancellationToken ct = default)
    {
        return await userService.UpdateAsync(
            command.UserId,
            command.Request.FirstName,
            command.Request.LastName,
            ct);
    }
}
