namespace Application.Features.Employees.Queries.GetAll;

internal sealed class GetAllEmployeeQueryValidator
    : AbstractValidator<GetAllEmployeeQuery>
{
    public GetAllEmployeeQueryValidator()
    {
        RuleFor(x => x.JobTitle)
            .MaximumLength(100)
            .When(x => x.JobTitle is not null);

        RuleFor(x => x.Role)
            .MaximumLength(100)
            .When(x => x.Role is not null);

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .When(x => x.DepartmentId.HasValue);

        RuleFor(x => x.IsActive)
            .NotNull()
            .When(x => x.IsActive.HasValue);

        RuleFor(x => x.HireDateMin)
            .LessThanOrEqualTo(x => x.HireDateMax)
            .When(x => x.HireDateMin.HasValue && x.HireDateMax.HasValue);

        RuleFor(x => x.HireDateMax)
            .GreaterThanOrEqualTo(x => x.HireDateMin)
            .When(x => x.HireDateMin.HasValue && x.HireDateMax.HasValue);

        RuleFor(x => x.LastLoginDateMin)
            .LessThanOrEqualTo(x => x.LastLoginDateMax)
            .When(x => x.LastLoginDateMin.HasValue && x.LastLoginDateMax.HasValue);

        RuleFor(x => x.LastLoginDateMax)
            .GreaterThanOrEqualTo(x => x.LastLoginDateMin)
            .When(x => x.LastLoginDateMin.HasValue && x.LastLoginDateMax.HasValue);

        RuleFor(x => x.CreatedAtMin)
            .LessThanOrEqualTo(x => x.CreatedAtMax)
            .When(x => x.CreatedAtMin.HasValue && x.CreatedAtMax.HasValue);

        RuleFor(x => x.CreatedAtMax)
            .GreaterThanOrEqualTo(x => x.CreatedAtMin)
            .When(x => x.CreatedAtMin.HasValue && x.CreatedAtMax.HasValue);

        RuleFor(x => x.UserType)
            .IsInEnum()
            .When(x => x.UserType.HasValue);
    }
}
