namespace Application.Features.EmployeeDepartments.Commands;

public sealed record EmployeeDepartmentCommand(
    Guid DepartmentId,
    Guid EmployeeId
    ) : ICommand;

internal sealed class EmployeeDepartmentCommandValidator  : AbstractValidator<EmployeeDepartmentCommand>
{
    public EmployeeDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department ID is required.");
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");
    }
}

