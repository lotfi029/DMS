namespace Application.Features.Employees.Queries.GetAll;

internal sealed class GetAllEmployeeQueryValidator
    : AbstractValidator<GetAllEmployeeQuery>
{
    public GetAllEmployeeQueryValidator()
    {
        RuleFor(x => x.Request.JobTitle)
            .MaximumLength(100)
            .When(x => x.Request.JobTitle is not null);

        RuleFor(x => x.Request.RoleIds)
            .NotEmpty()
            .When(x => x.Request.RoleIds is not null);

        RuleFor(x => x.Request.DepartmentIds)
            .NotEmpty()
            .When(x => x.Request.DepartmentIds is not null);

        RuleFor(x => x.Request.IsActive)
            .NotNull()
            .When(x => x.Request.IsActive.HasValue);

        RuleFor(x => x.Request.HireDateMin)
            .LessThanOrEqualTo(x => x.Request.HireDateMax)
            .When(x => x.Request.HireDateMin.HasValue && x.Request.HireDateMax.HasValue);

        RuleFor(x => x.Request.HireDateMax)
            .GreaterThanOrEqualTo(x => x.Request.HireDateMin)
            .When(x => x.Request.HireDateMin.HasValue && x.Request.HireDateMax.HasValue);

        RuleFor(x => x.Request.LastLoginDateMin)
            .LessThanOrEqualTo(x => x.Request.LastLoginDateMax)
            .When(x => x.Request.LastLoginDateMin.HasValue && x.Request.LastLoginDateMax.HasValue);

        RuleFor(x => x.Request.LastLoginDateMax)
            .GreaterThanOrEqualTo(x => x.Request.LastLoginDateMin)
            .When(x => x.Request.LastLoginDateMin.HasValue && x.Request.LastLoginDateMax.HasValue);

        RuleFor(x => x.Request.CreatedAtMin)
            .LessThanOrEqualTo(x => x.Request.CreatedAtMax)
            .When(x => x.Request.CreatedAtMin.HasValue && x.Request.CreatedAtMax.HasValue);

        RuleFor(x => x.Request.CreatedAtMax)
            .GreaterThanOrEqualTo(x => x.Request.CreatedAtMin)
            .When(x => x.Request.CreatedAtMin.HasValue && x.Request.CreatedAtMax.HasValue);

        RuleFor(x => x.Request.UserType)
            .IsInEnum()
            .When(x => x.Request.UserType.HasValue);
    }
}
