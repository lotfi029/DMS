namespace Application.Features.Departments.Commands.Update;

public sealed record UpdateDepartmentCommand(
    Guid Id,
    UpdateDepartmentRequest Request) : ICommand;

public sealed class UpdateDepartmentCommandHandler(
    IDepartmentDomainService domainService) : ICommandHandler<UpdateDepartmentCommand>
{
    public async Task<Result> HandleAsync(UpdateDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await domainService.UpdateAsync(command.Id, command.Request.Name, command.Request.Description, ct);

        if (result.IsFailure)
            return result.Error;

        return Result.Success();
    }
}
