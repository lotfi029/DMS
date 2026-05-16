namespace Application.Features.Permissions.Commands.RevokeOverride;

internal sealed class RevokePermissionOverrideCommandValidator : AbstractValidator<RevokePermissionOverrideCommand>
{
    public RevokePermissionOverrideCommandValidator()
    {
        RuleFor(x => x.TargetUserId)
            .NotEmpty().WithMessage("Target user ID is required.");

        RuleFor(x => x.Permission)
            .NotEmpty().WithMessage("Permission is required.");

        RuleFor(x => x.CallerUserId)
            .NotEmpty().WithMessage("Caller user ID is required.");
    }
}
