namespace Application.Features.Departments.Commands.RemoveUser;

public sealed record RemoveUserFromDepartmentCommand(string UserId, Guid DepartmentId) : ICommand;

internal sealed class RemoveUserCommandHandler(IDepartmentDomainService departmentDomainService) : ICommandHandler<RemoveUserFromDepartmentCommand>
{
    public async Task<Result> HandleAsync(RemoveUserFromDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.RemoveUserAsync(command.UserId, command.DepartmentId, ct);
        if (result.IsFailure)
            return result.Error;
        return Result.Success();
    }
}