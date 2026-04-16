namespace Application.Features.Departments.Commands.AddUser;

public sealed record AddUserToDepartmentCommand(string UserId, Guid DepartmentId) : ICommand;

internal sealed class AddUserCommandHandler(IDepartmentDomainService departmentDomainService) : ICommandHandler<AddUserToDepartmentCommand>
{
    public async Task<Result> HandleAsync(AddUserToDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.AddUserAsync(command.UserId, command.DepartmentId, ct);
        if (result.IsFailure)
            return result.Error;
        return Result.Success();
    }
}