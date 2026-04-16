namespace Application.Features.Departments.Commands.MoveUser;

public sealed record MoveUserToDepartmentCommand(string UserId, Guid ToDepartmentId) : ICommand;

internal sealed class MoveUserCommandHandler(IDepartmentDomainService departmentDomainService) : ICommandHandler<MoveUserToDepartmentCommand>
{
    public async Task<Result> HandleAsync(MoveUserToDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.MoveUserAsync(command.UserId, command.ToDepartmentId, ct);
        if (result.IsFailure)
            return result.Error;
        return Result.Success();
    }
}