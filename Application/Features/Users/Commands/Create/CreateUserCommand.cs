using System.Text.Json;

namespace Application.Features.Users.Commands.Create;

public sealed record CreateUserCommand(AddUserRequest Request) : ICommand<string>;

public sealed class CreateUserCommandHandler(
    IAuthService authService,
    IAuditService auditService,
    ILogger<CreateUserCommandHandler> logger) : ICommandHandler<CreateUserCommand, string>
{
    public async Task<Result<string>> HandleAsync(CreateUserCommand command, CancellationToken ct = default)
    {
        logger.LogInformation(LogMessages.User_Created, command.Request.UserName, command.Request.Email);

        var newUser = new RegisterRequest(
            command.Request.FirstName,
            command.Request.LastName,
            command.Request.Password,
            command.Request.Email,
            command.Request.UserName
        );

        var registerResult = await authService.RegisterAsync(command.Request.RoleId! ?? string.Empty, UserType.Employee, newUser, ct);

        if (registerResult.IsFailure)
        {
            logger.LogWarning(LogMessages.User_CreateFailed, command.Request.UserName, registerResult.Error.Description);
            return registerResult.Error;
        }

        logger.LogInformation(LogMessages.User_Created, registerResult.Value, command.Request.UserName);

        await auditService.LogActionAsync(
            action: AuditAction.UserCreated,
            module: AuditModules.Users,
            entityName: AuditEntityNames.User,
            entityId: registerResult.Value,
            description: $"User '{command.Request.UserName}' created.",
            newValues: JsonSerializer.Serialize(new
            {
                command.Request.FirstName,
                command.Request.LastName,
                command.Request.Email,
                command.Request.UserName
            }),
            ct: ct);

        return registerResult.Value!;
    }
}