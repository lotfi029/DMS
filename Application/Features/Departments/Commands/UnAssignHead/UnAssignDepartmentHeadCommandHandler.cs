namespace Application.Features.Departments.Commands.UnAssignHead;
internal sealed class UnAssignDepartmentHeadCommandHandler(
    IDepartmentRepository repo,
    IAuditService auditService
    ) : ICommandHandler<DepartmentCommand>
{
    public async Task<Result> HandleAsync(DepartmentCommand command, CancellationToken ct = default)
    {
        if (await repo.ExistsAsync(e => e.Id == command.Id, ct: ct))
            return DepartmentErrors.NotFound;

        var unassignResult = await repo.ExecuteUpdateAsync(
            x => x.Id == command.Id,
            d => d.SetProperty(x => x.DepartmentHeadId, v => null),
            ct
            );

        if (unassignResult == 0)
            return DepartmentErrors.NotFound;

        await auditService.LogActionAsync(
            action: AuditAction.RemovedDepartmentHead,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.Id.ToString(),
            description: $"Unassigned department head from department with ID {command.Id}",
            outcome: AuditOutcome.Success,
            ct: ct
            );

        return Result.Success();
    }
}