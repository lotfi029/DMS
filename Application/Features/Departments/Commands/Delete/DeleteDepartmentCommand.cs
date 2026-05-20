namespace Application.Features.Departments.Commands.Delete;
internal sealed class DeleteDepartmentCommandHandler(
    IDepartmentDomainService domainService,
    IAuditService auditService,
    ILogger<DeleteDepartmentCommandHandler> logger) : ICommandHandler<DepartmentCommand>
{
    public async Task<Result> HandleAsync(DepartmentCommand command, CancellationToken ct = default)
    {
        var result = await domainService.DeleteAsync(command.Id, ct);

        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.Dept_NotFound, command.Id);
            return result.Error;
        }

        logger.LogInformation(LogMessages.Dept_Deleted, command.Id);

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentDeleted,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.Id.ToString(),
            description: $"Department '{command.Id}' deleted.",
            ct: ct);

        return Result.Success();
    }
}
