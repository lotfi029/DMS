namespace Application.Features.Departments.Commands.Activate;

public sealed record ActiveDepartmentCommand(Guid Id) : ICommand;

internal sealed class ActivateDepartmentCommandHandler(
    IDepartmentDomainService departmentService,
    IAuditService auditService,
    ILogger<ActivateDepartmentCommandHandler> logger
    ) : ICommandHandler<ActiveDepartmentCommand>
{
    public async Task<Result> HandleAsync(ActiveDepartmentCommand command, CancellationToken ct = default)
    {
        if (await departmentService.ActivateAsync(command.Id, ct) is { IsFailure: true} result)
        {
            logger.LogWarning(
                "Failed to activate department with ID {DepartmentId}. Error: {ErrorMessage}", 
                command.Id, result.Error.ToString());
            return Result.Failure(result.Error);
        }

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentActivated,
            module: AuditModules.Departments,
            entityName: AuditModules.Departments,
            entityId: command.Id.ToString(),
            description: $"Department activated with ID {command.Id}",
            outcome: AuditOutcome.Success,
            ct: ct);
            
        return Result.Success();
    }
}