namespace Application.Features.Users.Commands.Activate;

public sealed record ActivateUserCommand(string UserId) : ICommand;

public sealed class ActivateUserCommandHandler(
    IUserDomainService userService,
    IAuditService auditService) : ICommandHandler<ActivateUserCommand>
{
    public async Task<Result> HandleAsync(ActivateUserCommand command, CancellationToken ct = default)
    {
        
        var result = await userService.ActivateAsync(command.UserId, ct);
        if (result.IsFailure)
            return result.Error;

        await auditService.LogActionAsync(
            AuditAction.UserActivated,
            "Users", "ApplicationUser",
            entityId: command.UserId,
            description: $"User '{command.UserId}' activated.", ct: ct);

        return result;
    }
}
