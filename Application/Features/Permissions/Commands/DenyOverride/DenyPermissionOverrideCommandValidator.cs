namespace Application.Features.Permissions.Commands.DenyOverride;

internal sealed class DenyPermissionOverrideCommandValidator : AbstractValidator<DenyPermissionOverrideCommand>
{
    public DenyPermissionOverrideCommandValidator()
    {
        RuleFor(x => x.TargetUserId)
            .NotEmpty()
            .WithMessage("Target user ID is required.");
        RuleFor(x => x.Permission)
            .NotEmpty()
            .WithMessage("Permission is required.")
            .Must(p => DefaultPermissions.AllDefaultPermissions.Contains(p))
            .WithMessage(x => $"'{x.Permission}' is not a valid permission string.");


        RuleFor(x => x.CallerUserId)
            .NotEmpty()
            .WithMessage("Caller user ID is required.");


        RuleFor(x => x.Reason)
            .MaximumLength(500)
            .When(x => x.Reason is not null);
    }
}
