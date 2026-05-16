namespace Application.Features.Departments.Commands.AssignHead;

internal sealed class AssignDepartmentHeadCommandValidator : AbstractValidator<AssignDepartmentHeadCommand>
{
    public AssignDepartmentHeadCommandValidator()
    {
        RuleFor(x => x.DepartmentId).NotEmpty().WithMessage("Department ID is required.");
        RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Employee ID is required.");
    }
}
