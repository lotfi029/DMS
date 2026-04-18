namespace Application.Features.Users.Commands.Delete;

public sealed record DeleteUserCommand(string UserId) : ICommand;
// TODO: add soft delete and remove from user service
public sealed class DeleteUserCommandHandler(
    IUserDomainService userService,
    IAuditService auditService,
    ILogger<DeleteUserCommandHandler> logger) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> HandleAsync(DeleteUserCommand command, CancellationToken ct = default)
    {
        var result = await userService.DeactivateAsync(command.UserId, ct);

        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.User_NotFound, command.UserId);
            return result.Error;
        }

        logger.LogInformation(LogMessages.User_Deleted, command.UserId);

        await auditService.LogActionAsync(
            action: AuditAction.UserDeleted,
            module: AuditModules.Users,
            entityName: AuditEntityNames.User,
            entityId: command.UserId,
            description: $"User '{command.UserId}' soft-deleted.",
            ct: ct);

        return Result.Success();
    }
}
