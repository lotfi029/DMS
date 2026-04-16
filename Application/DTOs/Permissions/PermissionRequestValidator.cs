namespace Application.DTOs.Permissions;

public sealed class PermissionRequestValidator : AbstractValidator<PermissionRequest>
{
    public PermissionRequestValidator()
    {
        RuleFor(x => x.PermissionName)
            .NotEmpty().WithMessage("Permission name is required.")
            .MaximumLength(100).WithMessage("Permission name must not exceed 100 characters.");
    }
}

