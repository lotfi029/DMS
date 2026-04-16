namespace Application.Features.Departments.Commands.Create;

public sealed record CreateDepartmentCommand(
    CreateDepartmentRequest Request) : ICommand;

public sealed class CreateDepartmentCommandHandler(
    IDepartmentDomainService domainService,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateDepartmentCommand>
{
    public async Task<Result> HandleAsync(CreateDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await domainService.CreateAsync(command.Request.Name, command.Request.Description, ct);

        if (result.IsFailure)
            return result.Error;

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
