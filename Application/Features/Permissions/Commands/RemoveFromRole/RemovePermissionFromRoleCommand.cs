namespace Application.Features.Permissions.Commands.RemoveFromRole;

public sealed record RemovePermissionFromRoleCommand(string RoleId, string Permission) : ICommand;

internal sealed class RemovePermissionFromRoleCommandValidator : AbstractValidator<RemovePermissionFromRoleCommand>
{
    public RemovePermissionFromRoleCommandValidator()
    {
        RuleFor(x => x.Permission)
            .NotEmpty().WithMessage("Permission name is required.")
            .MaximumLength(100).WithMessage("Permission name must not exceed 100 characters.");
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("Role ID is required.")
            .Must(BeAValidGuid).WithMessage("Role ID must be a valid GUID.");
    }
    private bool BeAValidGuid(string roleId)
    {
        return Guid.TryParse(roleId, out _);
    }
}
internal sealed class RemovePermissionFromRoleCommandHandler(
    IPermissionService service,
    IAuditService auditService) : ICommandHandler<RemovePermissionFromRoleCommand>
{
    public async Task<Result> HandleAsync(RemovePermissionFromRoleCommand command, CancellationToken ct = default)
    {
        var result = await service.RemovePermissionFromRoleAsync(command.RoleId, command.Permission, ct);

        if (result.IsFailure)
            return result.Error;

        await auditService.LogActionAsync(
            action: AuditAction.PermissionRemovedFromRole,
            module: AuditModules.Permissions,
            entityName: AuditEntityNames.RoleClaim,
            ct: ct);

        return Result.Success();
    }
}
