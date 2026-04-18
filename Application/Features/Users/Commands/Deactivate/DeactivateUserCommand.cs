namespace Application.Features.Users.Commands.Deactivate;

public sealed record DeactivateUserCommand(string UserId) : ICommand;

public sealed class DeactivateUserCommandHandler(
    IUserDomainService userService,
    IAuditService auditService) : ICommandHandler<DeactivateUserCommand>
{
    public async Task<Result> HandleAsync(DeactivateUserCommand command, CancellationToken ct = default)
    {
        var result = await userService.DeactivateAsync(command.UserId, ct);
        
        if (result.IsFailure)
            return result.Error;

        await auditService.LogActionAsync(
            AuditAction.UserActivated, // or UserDeactivated
            "Users", "ApplicationUser",
            entityId: command.UserId,
            description: $"User '{command.UserId}' deactivated.", ct: ct);

        return result;
    }
}