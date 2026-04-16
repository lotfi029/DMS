namespace Application.DTOs.Roles;

public sealed class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty()
            .WithMessage("Role name is required.")
            .MaximumLength(256)
            .WithMessage("Role name must not exceed 256 characters.");
    }
}
