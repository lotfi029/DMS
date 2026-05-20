namespace Application.Features.EmployeeDepartments.Commands.RemoveUser;
internal sealed class RemoveEmployeeFromDepartmentCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    ILogger<RemoveEmployeeFromDepartmentCommandHandler> logger) : ICommandHandler<EmployeeDepartmentCommand>
{
    public async Task<Result> HandleAsync(EmployeeDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.RemoveEmployeeAsync(command.EmployeeId, command.DepartmentId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Dept_UserRemoved, command.EmployeeId, command.DepartmentId);

        await auditService.LogActionAsync(
            action: AuditAction.UserRemovedFromDepartment,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.DepartmentId.ToString(),
            description: $"User '{command.EmployeeId}' removed from department '{command.DepartmentId}'.",
            ct: ct);

        return Result.Success();
    }
}
