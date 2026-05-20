namespace Application.Features.Departments.Commands.Activate;

internal sealed class ActivateDepartmentCommandHandler(
    IDepartmentDomainService departmentService,
    IAuditService auditService,
    ILogger<ActivateDepartmentCommandHandler> logger
    ) : ICommandHandler<DepartmentCommand>
{
    public async Task<Result> HandleAsync(DepartmentCommand command, CancellationToken ct = default)
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