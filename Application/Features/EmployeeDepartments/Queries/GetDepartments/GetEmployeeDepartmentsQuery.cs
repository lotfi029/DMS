using Application.Abstractions.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.EmployeeDepartments.Queries.GetDepartments;

public sealed record GetEmployeeDepartmentsQuery(
    Guid EmployeeId) : IQuery<List<DepartmentListResponse>>;


internal sealed class GetEmployeeDepartmentsQueryValidator : AbstractValidator<GetEmployeeDepartmentsQuery>
{
    public GetEmployeeDepartmentsQueryValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.")
            .Must(BeAValidGuid).WithMessage("Employee ID must be a valid GUID.");
    }
    private bool BeAValidGuid(Guid employeeId)
    {
        return Guid.TryParse(employeeId.ToString(), out _);
    }
}

internal sealed class GetEmployeeDepartmentsQueryHandler(
    IApplicationDbContext context) : IQueryHandler<GetEmployeeDepartmentsQuery, List<DepartmentListResponse>>
{
    public async Task<Result<List<DepartmentListResponse>>> HandleAsync(GetEmployeeDepartmentsQuery query, CancellationToken ct = default)
    {
        var departments = await context.EmployeeDepartments
            .Where(ed => ed.EmployeeId == query.EmployeeId)
            .Select(ed => new DepartmentListResponse(
                ed.Department.Id,
                ed.Department.Name,
                ed.Department.CreatedAt)
            )
            .ToListAsync(ct);

        return departments;
    }
}