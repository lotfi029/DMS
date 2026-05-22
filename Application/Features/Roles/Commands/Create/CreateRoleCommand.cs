namespace Application.Features.Roles.Commands.Create;

public sealed record CreateRoleCommand(string RoleName) : ICommand;

internal sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name is required.")
            .MaximumLength(256).WithMessage("Role name must not exceed 256 characters.")
            .Matches(@"^[a-zA-Z0-9_\-]+$")
            .WithMessage("Role name can only contain letters, numbers, underscores, and hyphens.");
    }
}

internal sealed class CreateRoleCommandHandler(
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
