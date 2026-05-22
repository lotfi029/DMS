namespace Application.Features.Permissions.Commands.AssignToRole;

public sealed record AssignPermissionToRoleCommand(string RoleId, string Permission) : ICommand;

internal sealed class AssignPermissionToRoleCommandValidator : AbstractValidator<AssignPermissionToRoleCommand>
{
    public AssignPermissionToRoleCommandValidator()
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
internal sealed class AssignPermissionToRoleCommandHandler(
    IPermissionService service,
    IAuditService auditService
    ) : ICommandHandler<AssignPermissionToRoleCommand>
{
    public async Task<Result> HandleAsync(AssignPermissionToRoleCommand command, CancellationToken ct = default)
    {
        var result = await service.AssignPermissionToRoleAsync(command.RoleId, command.Permission, ct);

        if (result.IsFailure)
            return result.Error;

        await auditService.LogActionAsync(
            action: AuditAction.PermissionAssignedToRole,
            module: AuditModules.Permissions,
            entityName: AuditEntityNames.RoleClaim,
            ct: ct);

        return Result.Success();
    }
}