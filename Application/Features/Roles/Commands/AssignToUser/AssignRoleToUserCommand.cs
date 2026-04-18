namespace Application.Features.Roles.Commands.AssignToUser;

public sealed record AssignRoleToUserCommand(string UserId, string RoleId) : ICommand;

public sealed class AssignRoleToUserCommandHandler(
    IRoleService service,
    IAuditService auditService,
    ILogger<AssignRoleToUserCommandHandler> logger) : ICommandHandler<AssignRoleToUserCommand>
{
    public async Task<Result> HandleAsync(AssignRoleToUserCommand command, CancellationToken ct = default)
    {
        var result = await service.AssignRoleToUserAsync(command.UserId, command.RoleId, ct);

        if (result.IsFailure)
            return result.Error;

        logger.LogInformation(LogMessages.Role_AssignedToUser, command.RoleId, command.UserId);

        await auditService.LogActionAsync(
            action: AuditAction.RoleAssignedToUser,
            module: AuditModules.Roles,
            entityName: AuditEntityNames.User,
            entityId: command.UserId,
            description: $"Role '{command.RoleId}' assigned to user '{command.UserId}'.",
            ct: ct);

        return Result.Success();
    }
}
