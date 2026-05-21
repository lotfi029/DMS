using Application.Abstractions.Data;
using Application.Abstractions.Pagination;
using Application.DTOs.Employees;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.EmployeeDepartments.Queries.GetEmployees;

public sealed record GetDepartmentEmployeesQuery(
    Guid DepartmentId,
    int pagedNumber,
    int pageSize) : IQuery<PagedResult<EmployeeListResponse>>;

internal sealed class GetDepartmentEmployeesQueryHandler(
    IApplicationDbContext context) : IQueryHandler<GetDepartmentEmployeesQuery, PagedResult<EmployeeListResponse>>
{
    public async Task<Result<PagedResult<EmployeeListResponse>>> HandleAsync(GetDepartmentEmployeesQuery query, CancellationToken ct = default)
    {
        var employees = await context.EmployeeProfiles
            .AsNoTracking()
            .Where(x => x.DepartmentId == query.DepartmentId)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(ct);

        if (employees is null || employees.Count == 0)
            return Result.Success(PagedResult<EmployeeListResponse>.Default);

        var items = employees
            .GroupBy(x => new
            {
                x.EmployeeId,
                x.UserId,
                x.FirstName,
                x.LastName,
                x.Email,
                x.JobTitle,
                x.EmployeeIsActive
            })
            .Select(g => new EmployeeListResponse(
                g.Key.EmployeeId,
                g.Key.UserId,
                g.Key.FirstName,
                g.Key.LastName,
                $"{g.Key.FirstName} {g.Key.LastName}",
                g.Key.Email,
                g.Key.JobTitle,
                g.Key.EmployeeIsActive,
                g.SelectMany(x => new[] { new DepartmentFromEmployeeResponse (
                    x.DepartmentId!.Value,
                    x.DepartmentName!
                )}),
                g.SelectMany(x => new[] { new RoleForEmployeeResponse(x.UserRoleId!, x.RoleName!) })
            ))
            .ToList();

        return new PagedResult<EmployeeListResponse>(items, query.pagedNumber, items.Count, query.pageSize);

    }
}