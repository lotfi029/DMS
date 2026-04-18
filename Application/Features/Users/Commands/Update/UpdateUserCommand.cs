using System.Text.Json;

namespace Application.Features.Users.Commands.Update;

public sealed record UpdateUserCommand(string UserId, UpdateUserRequest Request) : ICommand;

public sealed class UpdateUserCommandHandler(
    IUserDomainService userService,
    IAuditService auditService,
    ILogger<UpdateUserCommandHandler> logger) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> HandleAsync(UpdateUserCommand command, CancellationToken ct = default)
    {
        var result = await userService.UpdateAsync(
            command.UserId,
            command.Request.FirstName,
            command.Request.LastName,
            ct);

        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.User_UpdateFailed, command.UserId, result.Error.Description);
            return result.Error;
        }

        logger.LogInformation(LogMessages.User_Updated, command.UserId);

        await auditService.LogActionAsync(
            AuditAction.UserUpdated, AuditModules.Users, AuditEntityNames.User,
            entityId: command.UserId,
            description: $"User '{command.UserId}' profile updated.",
            newValues: JsonSerializer.Serialize(command.Request), 
            ct: ct);

        return Result.Success();
    }
}
