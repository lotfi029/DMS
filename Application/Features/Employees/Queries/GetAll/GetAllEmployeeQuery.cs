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
    IEmployeeRepository repository) : IQueryHandler<GetAllEmployeeQuery, IEnumerable<EmployeeListResponse>>
{
    public async Task<Result<IEnumerable<EmployeeListResponse>>> HandleAsync(GetAllEmployeeQuery query, CancellationToken ct = default)
    {
        Expression<Func<Employee, bool>> queryExpression = x 
            => (!string.IsNullOrEmpty(query.JobTitle) || x.JobTitle == query.JobTitle) 
            || (!query.IsActive.HasValue || x.IsActive == query.IsActive);

        var employees = await repository.GetAllAsync(queryExpression, ["AppUser", "Department"], ct);

        if (!employees.Any())
            return Result.Success(Enumerable.Empty<EmployeeListResponse>());

        var response = employees.Adapt<List<EmployeeListResponse>>();

        return response;
    }
}