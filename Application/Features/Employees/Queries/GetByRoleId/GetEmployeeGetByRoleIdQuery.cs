using Application.DTOs.Employees;

namespace Application.Features.Employees.Queries.GetByRoleId;

public sealed record GetEmployeeGetByRoleIdQuery(string RoleId) : IQuery<IEnumerable<EmployeeListResponse>>;

internal sealed class GetEmployeeGetByRoleIdQueryValidator : AbstractValidator<GetEmployeeGetByRoleIdQuery>
{
    public GetEmployeeGetByRoleIdQueryValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();
    }
}
internal sealed class GetEmployeeGetByRoleIdQueryHandler(
    IEmployeeRepository employeeRepository,
    IEmployeeDepartmentRepository employeeDepartmentRepository) : IQueryHandler<GetEmployeeGetByRoleIdQuery, IEnumerable<EmployeeListResponse>>
{
    public async Task<Result<IEnumerable<EmployeeListResponse>>> HandleAsync(GetEmployeeGetByRoleIdQuery query, CancellationToken ct = default)
    {

        var employees = await employeeRepository.GetEmployeesByRoleAsync(roleId: query.RoleId, ct: ct);
        
        if (!employees.Any())
            return Result.Success(Enumerable.Empty<EmployeeListResponse>());
        
        var departments = await employeeDepartmentRepository
            .GetAllAsync(x => employees.Select(e => e.Id).Contains(x.EmployeeId), include: [nameof(EmployeeDepartment.Department)], ct: ct);
        
        var response = employees.Join(
            departments,
            e => e.Id,
            ed => ed.EmployeeId,
            (e, ed) => new { e, ed }
        )
        .GroupBy(x => x.e)
        .Select(g => new EmployeeListResponse(
            Id: g.Key.Id,
            UserId: g.Key.AppUser.Id,
            FirstName: g.Key.AppUser.FirstName,
            LastName: g.Key.AppUser.LastName,
            FullName: $"{g.Key.AppUser.FirstName} {g.Key.AppUser.LastName}",
            Email: g.Key.AppUser.Email!,
            JobTitle: g.Key.JobTitle,
            IsActive: g.Key.IsActive,
            Departments: [.. g.Select(x => new DepartmentFromEmployeeResponse(
                Id: x.ed.DepartmentId,
                Name: x.ed.Department.Name
            ))]
        )).ToList();
        return response;
    }
}