using Application.Abstractions.Data;
using Application.Abstractions.Pagination;
using Application.DTOs.Employees;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Employees.Queries.GetByRoleId;

public sealed record GetEmployeeGetByRoleIdQuery(
    string RoleId,
    int PageNumber,
    int PageSize) : IQuery<PagedResult<EmployeeListResponse>>;

internal sealed class GetEmployeeGetByRoleIdQueryValidator : AbstractValidator<GetEmployeeGetByRoleIdQuery>
{
    public GetEmployeeGetByRoleIdQueryValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty();
    }
}
internal sealed class GetEmployeeGetByRoleIdQueryHandler(
    IApplicationDbContext context) : IQueryHandler<GetEmployeeGetByRoleIdQuery, PagedResult<EmployeeListResponse>>
{
    public async Task<Result<PagedResult<EmployeeListResponse>>> HandleAsync(GetEmployeeGetByRoleIdQuery query, CancellationToken ct = default)
    {
        var employee = await context.EmployeeProfiles
            .AsNoTracking()
            .Where(x => x.UserRoleId == query.RoleId)
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(ct);

        var items = employee
            .GroupBy(e => new
            {
                e.EmployeeId,
                e.UserId,
                e.FirstName,
                e.LastName,
                e.Email,
                e.JobTitle,
                e.EmployeeIsActive
            })
            .Select(x => new EmployeeListResponse(
                x.Key.EmployeeId,
                x.Key.UserId,
                x.Key.FirstName,
                x.Key.LastName,
                $"{x.Key.FirstName} {x.Key.LastName}",
                x.Key.Email,
                x.Key.JobTitle,
                x.Key.EmployeeIsActive,
                x.SelectMany(x => x.DepartmentId.HasValue
                    ? new[] { new DepartmentFromEmployeeResponse(x.DepartmentId.Value, x.DepartmentName!) }
                    : []),
                x.SelectMany(x => x.UserRoleId != null
                    ? new[] { new RoleForEmployeeResponse(x.UserRoleId, x.RoleName!) }
                    : []
                )
            ))
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();


        return new PagedResult<EmployeeListResponse>(items, query.PageNumber, items.Count, query.PageSize);
    }
}