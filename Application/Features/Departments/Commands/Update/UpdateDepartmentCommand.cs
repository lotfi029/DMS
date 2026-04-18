namespace Application.Features.Departments.Commands.Update;

public sealed record UpdateDepartmentCommand(
    Guid Id,
    UpdateDepartmentRequest Request) : ICommand;

internal sealed class UpdateDepartmentCommandHandler(
    IDepartmentDomainService domainService,
    IAuditService auditService,
    ILogger<UpdateDepartmentCommandHandler> logger) : ICommandHandler<UpdateDepartmentCommand>
{
    public async Task<Result> HandleAsync(UpdateDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await domainService.UpdateAsync(command.Id, command.Request.Name, command.Request.Description, ct);

        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.Dept_NotFound, command.Id);
            return result.Error;
        }

        logger.LogInformation(LogMessages.Dept_Updated, command.Id);

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentUpdated,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.Id.ToString(),
            description: $"Department '{command.Id}' updated.",
            ct: ct);

        return Result.Success();
    }
}

