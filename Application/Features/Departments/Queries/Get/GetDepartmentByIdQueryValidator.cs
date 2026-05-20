namespace Application.Features.Departments.Queries.Get;

internal sealed class GetDepartmentByIdQueryValidator : AbstractValidator<GetDepartmentByIdQuery>
{
    public GetDepartmentByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Department ID is required.");
    }
}
