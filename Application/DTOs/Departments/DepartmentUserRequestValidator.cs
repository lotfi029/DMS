namespace Application.DTOs.Departments;

public sealed class DepartmentUserRequestValidator : AbstractValidator<DepartmentUserRequest>
{
    public DepartmentUserRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");
    }
}