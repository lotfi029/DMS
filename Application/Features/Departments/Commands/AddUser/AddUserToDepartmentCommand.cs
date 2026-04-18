namespace Application.Features.Departments.Commands.AddUser;

public sealed record AddUserToDepartmentCommand(string UserId, Guid DepartmentId) : ICommand;

internal sealed class AddUserCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    ILogger<AddUserCommandHandler> logger) : ICommandHandler<AddUserToDepartmentCommand>
{
    public async Task<Result> HandleAsync(AddUserToDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.AddUserAsync(command.UserId, command.DepartmentId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Dept_UserAdded, command.UserId, command.DepartmentId);

        await auditService.LogActionAsync(
            action: AuditAction.UserAddedToDepartment,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.DepartmentId.ToString(),
            description: $"User '{command.UserId}' added to department '{command.DepartmentId}'.",
            ct: ct);

        return Result.Success();
    }
}