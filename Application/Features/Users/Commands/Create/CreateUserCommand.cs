using System.Text.Json;

namespace Application.Features.Users.Commands.Create;

public sealed record CreateUserCommand(AddUserRequest Request) : ICommand<string>;

public sealed class CreateUserCommandHandler(
    IAuthService authService,
    IAuditService auditService,
    IDepartmentDomainService departmentDomainService,
    IDepartmentRepository departmentRepository,
    ILogger<CreateUserCommandHandler> logger) : ICommandHandler<CreateUserCommand, string>
{
    public async Task<Result<string>> HandleAsync(CreateUserCommand command, CancellationToken ct = default)
    {
        logger.LogInformation(LogMessages.User_Created, command.Request.UserName, command.Request.Email);

        if (await ValidatedAsync(command, ct) is { IsFailure: true } errors)
            return errors.Error;

        var newUser = new RegisterRequest(
            command.Request.FirstName,
            command.Request.LastName,
            command.Request.Password,
            command.Request.Email,
            command.Request.UserName
        );

        var registerResult = await authService.RegisterAsync(command.Request.RoleId! ?? string.Empty, newUser, ct);

        if (registerResult.IsFailure)
        {
            logger.LogWarning(LogMessages.User_CreateFailed, command.Request.UserName, registerResult.Error.Description);
            return registerResult.Error;
        }

        if (command.Request.DepartmentId.HasValue)
        {
            var departmentResult = await departmentDomainService
                .AddUserAsync(
                    registerResult.Value!,
                    command.Request.DepartmentId.Value,
                    ct
                );

            if (departmentResult.IsFailure)
                return departmentResult.Error;
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

    private async Task<Result> ValidatedAsync(CreateUserCommand command, CancellationToken ct = default)
    {
        var departmentExists = command.Request.DepartmentId.HasValue &&
            await departmentRepository.ExistsAsync(x => x.Id == command.Request.DepartmentId, ct) || !command.Request.DepartmentId.HasValue;

        if (!departmentExists)
            return DepartmentErrors.UserNotInDepartment;

        return Result.Success();
    }
}