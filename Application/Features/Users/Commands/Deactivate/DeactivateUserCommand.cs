namespace Application.Features.Users.Commands.Deactivate;

public sealed record DeactivateUserCommand(string UserId) : ICommand;

public sealed class DeactivateUserCommandHandler(
    IUserDomainService userService,
    IAuditService auditService,
    ILogger<DeactivateUserCommandHandler> logger) : ICommandHandler<DeactivateUserCommand>
{
    public async Task<Result> HandleAsync(DeactivateUserCommand command, CancellationToken ct = default)
    {
        var result = await userService.DeactivateAsync(command.UserId, ct);

        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.User_NotFound, command.UserId);
            return result.Error;
        }

        logger.LogInformation(LogMessages.User_Deactivated, command.UserId);

        await auditService.LogActionAsync(
            AuditAction.UserDeactivated,
            AuditModules.Users, AuditEntityNames.User,
            entityId: command.UserId,
            description: $"User '{command.UserId}' deactivated.", ct: ct);

        return result;
    }
}