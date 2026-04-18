namespace Application.Features.Departments.Commands.Create;

public sealed record CreateDepartmentCommand(CreateDepartmentRequest Request) : ICommand;

internal sealed class CreateDepartmentCommandHandler(
    IDepartmentDomainService domainService,
    IUnitOfWork unitOfWork,
    IAuditService auditService,
    ILogger<CreateDepartmentCommandHandler> logger) : ICommandHandler<CreateDepartmentCommand>
{
    public async Task<Result> HandleAsync(CreateDepartmentCommand command, CancellationToken ct = default)
    {
        logger.LogInformation(LogMessages.Dept_Creating, command.Request.Name);

        var result = await domainService.CreateAsync(command.Request.Name, command.Request.Description, ct);

        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.Dept_CreateFailed, command.Request.Name, result.Error.Description);
            return result.Error;
        }

        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation(LogMessages.Dept_Created, command.Request.Name);

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentCreated,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            description: $"Department '{command.Request.Name}' created.",
            ct: ct);

        return Result.Success();
    }
}