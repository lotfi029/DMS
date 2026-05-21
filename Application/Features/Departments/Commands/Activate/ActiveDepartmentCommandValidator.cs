namespace Application.Features.Departments.Commands.Activate;

public sealed class ActiveDepartmentCommandValidator : AbstractValidator<ActiveDepartmentCommand>
{
    public ActiveDepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Department ID is required.");
    }
}
