namespace Application.Features.Roles.Commands.Create;

public sealed record CreateRoleCommand(string RoleName) : ICommand;

public sealed class CreateRoleCommandHandler(
    IRoleService service,
    IAuditService auditService,
    ILogger<CreateRoleCommandHandler> logger) : ICommandHandler<CreateRoleCommand>
{
    public async Task<Result> HandleAsync(CreateRoleCommand command, CancellationToken ct = default)
    {

        logger.LogInformation(LogMessages.Role_Creating, command.RoleName);

        var result = await service.CreateRoleAsync(command.RoleName, ct);

        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.Role_CreateFailed, command.RoleName, result.Error.Description);
            return result.Error;
        }

        logger.LogInformation(LogMessages.Role_Created, command.RoleName);

        await auditService.LogActionAsync(
            action: AuditAction.RoleCreated,
            module: AuditModules.Roles,
            entityName: AuditEntityNames.Role,
            description: $"Role '{command.RoleName}' created.",
            ct: ct);

        return Result.Success();
    }
}
