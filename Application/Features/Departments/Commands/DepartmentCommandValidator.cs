namespace Application.Features.Departments.Commands;

public sealed class DepartmentCommandValidator : AbstractValidator<DepartmentCommand>
{
    public DepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Department ID is required.");
    }
}
