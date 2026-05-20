namespace Application.Features.EmployeeDepartments.Commands.AddUser;
internal sealed class AddUserCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    ILogger<AddUserCommandHandler> logger) : ICommandHandler<EmployeeDepartmentCommand>
{
    public async Task<Result> HandleAsync(EmployeeDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.AddEmployeeAsync(command.EmployeeId, command.DepartmentId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Dept_UserAdded, command.EmployeeId, command.DepartmentId);

        await auditService.LogActionAsync(
            action: AuditAction.UserAddedToDepartment,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.DepartmentId.ToString(),
            description: $"User '{command.EmployeeId}' added to department '{command.DepartmentId}'.",
            ct: ct);

        return Result.Success();
    }
}