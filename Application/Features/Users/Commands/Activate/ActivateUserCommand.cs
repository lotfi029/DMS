namespace Application.Features.Users.Commands.Activate;

public sealed record ActivateUserCommand(string UserId) : ICommand;

public sealed class ActivateUserCommandHandler(
    IUserDomainService userService,
    IAuditService auditService,
    ILogger<ActivateUserCommandHandler> logger) : ICommandHandler<ActivateUserCommand>
{
    public async Task<Result> HandleAsync(ActivateUserCommand command, CancellationToken ct = default)
    {        
        var result = await userService.ActivateAsync(command.UserId, ct);
        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.User_NotFound, command.UserId);
            return result.Error;
        }

        logger.LogInformation(LogMessages.User_Activated, command.UserId);
        
        await auditService.LogActionAsync(
            AuditAction.UserActivated,
            "Users", "ApplicationUser",
            entityId: command.UserId,
            description: $"User '{command.UserId}' activated.", ct: ct);

        return result;
    }
}
