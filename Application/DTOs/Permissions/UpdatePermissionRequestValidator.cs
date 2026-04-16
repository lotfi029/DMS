namespace Application.DTOs.Permissions;

public sealed class UpdatePermissionRequestValidator : AbstractValidator<UpdatePermissionRequest>
{
    public UpdatePermissionRequestValidator()
    {
        RuleFor(x => x.OldPermissionName)
            .NotEmpty().WithMessage("Old permission name is required.")
            .MaximumLength(100).WithMessage("Old permission name must not exceed 100 characters.");
        RuleFor(x => x.NewPermissionName)
            .NotEmpty().WithMessage("New permission name is required.")
            .MaximumLength(100).WithMessage("New permission name must not exceed 100 characters.");
    }
}

