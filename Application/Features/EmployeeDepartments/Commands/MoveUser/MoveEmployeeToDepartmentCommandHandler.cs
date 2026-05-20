namespace Application.Features.EmployeeDepartments.Commands.MoveUser;

internal sealed class MoveEmployeeToDepartmentCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    ILogger<MoveEmployeeToDepartmentCommandHandler> logger) : ICommandHandler<EmployeeDepartmentCommand>
{
    public async Task<Result> HandleAsync(EmployeeDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.MoveEmployeeAsync(command.EmployeeId, command.DepartmentId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Dept_UserMoved, command.EmployeeId, command.DepartmentId);

        await auditService.LogActionAsync(
            action: AuditAction.UserMovedBetweenDepartments,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.DepartmentId.ToString(),
            description: $"User '{command.EmployeeId}' moved to department '{command.DepartmentId}'.",
            ct: ct);

        return Result.Success();
    }
}