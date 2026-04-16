namespace Application.Features.Departments.Commands.Delete;

public sealed record DeleteDepartmentCommand(Guid Id) : ICommand;

public sealed class DeleteDepartmentCommandHandler(
    IDepartmentDomainService domainService) : ICommandHandler<DeleteDepartmentCommand>
{
    public async Task<Result> HandleAsync(DeleteDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await domainService.DeleteAsync(command.Id, ct);

        if (result.IsFailure)
            return result.Error;

        return Result.Success();
    }
}
