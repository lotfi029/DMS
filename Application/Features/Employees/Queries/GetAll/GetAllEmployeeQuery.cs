using Application.DTOs.Employees;
using System.Linq.Expressions;

namespace Application.Features.Employees.Queries.GetAll;

public sealed record GetAllEmployeeQuery(
    string? JobTitle,
    string? Role,
    Guid? DepartmentId,
    bool? IsActive,
    DateOnly? HireDateMin,
    DateOnly? HireDateMax,
    DateTime? LastLoginDateMin,
    DateTime? LastLoginDateMax,
    DateTime? CreatedAtMin,
    DateTime? CreatedAtMax,
    UserType? UserType) : IQuery<IEnumerable<EmployeeListResponse>>;
public record EmployeeQueryRequest(
    string? JobTitle,
    string? Role,
    Guid? DepartmentId,
    bool? IsActive,
    DateOnly? HireDateMin,
    DateOnly? HireDateMax,
    DateTime? LastLoginDateMin,
    DateTime? LastLoginDateMax,
    DateTime? CreatedAtMin,
    DateTime? CreatedAtMax,
    UserType? UserType
    );
internal sealed class GetAllEmployeeQueryHandler(
    IEmployeeRepository repository,
    IEmployeeDepartmentRepository employeeDepartmentRepository) : IQueryHandler<GetAllEmployeeQuery, IEnumerable<EmployeeListResponse>>
{
    public async Task<Result<IEnumerable<EmployeeListResponse>>> HandleAsync(GetAllEmployeeQuery query, CancellationToken ct = default)
    {
        Expression<Func<Employee, bool>> queryExpression = x
            => (string.IsNullOrEmpty(query.JobTitle) || x.JobTitle == query.JobTitle)
            && (!query.IsActive.HasValue || x.IsActive == query.IsActive);

        var employees = await repository.GetAllAsync(
            queryExpression, [nameof(Employee.AppUser)],
            ct);

        if (!employees.Any())
            return Result.Success(Enumerable.Empty<EmployeeListResponse>());

        var departments = await employeeDepartmentRepository.GetAllAsync(x => employees.Select(e => e.Id).Contains(x.EmployeeId), [nameof(EmployeeDepartment.Department)], ct);

        var response = employees.Join(
            departments,
            e => e.Id,
            ed => ed.EmployeeId,
            (e, ed) => new EmployeeListResponse(
                e.Id,
                e.AppUser.Id,
                e.AppUser.FirstName,
                e.AppUser.LastName,
                e.AppUser.Email!,
                e.JobTitle,
                e.IsActive,
                ed.DepartmentId,
                ed.Department.Name
            )).ToList();

        return response;
    }
}