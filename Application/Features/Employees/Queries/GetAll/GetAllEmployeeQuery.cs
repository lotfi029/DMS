using Application.Abstractions.Data;
using Application.DTOs.Employees;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Employees.Queries.GetAll;

public sealed record GetAllEmployeeQuery(
    EmployeeQueryRequest Request) : IQuery<IEnumerable<EmployeeListResponse>>;

internal sealed class GetAllEmployeeQueryHandler(
    IApplicationDbContext dbContext,
    IAuditService auditService) : IQueryHandler<GetAllEmployeeQuery, IEnumerable<EmployeeListResponse>>
{
    public async Task<Result<IEnumerable<EmployeeListResponse>>> HandleAsync(GetAllEmployeeQuery query, CancellationToken ct = default)
    {
        var q = from e in dbContext.Employees.AsNoTracking()
                    join u in dbContext.Users.AsNoTracking() on e.AppUserId equals u.Id

                    from ed in dbContext.EmployeeDepartments.AsNoTracking()
                        .Where(ed => ed.EmployeeId == e.Id)
                        .DefaultIfEmpty()
                    from d in dbContext.Departments.AsNoTracking()
                        .Where(d => ed.DepartmentId == d.Id)
                        .DefaultIfEmpty()
                    select new { Employee = e, User = u, Department = d };

        if (!string.IsNullOrEmpty(query.Request.JobTitle))
            q = q.Where(x => x.Employee.JobTitle.Contains(query.Request.JobTitle));

        if (query.Request.RoleIds != null && query.Request.RoleIds.Any())
            q = q.Where(x => dbContext.UserRoles.Any(ur => ur.UserId == x.User.Id && query.Request.RoleIds.Contains(ur.RoleId)));

        if (query.Request.DepartmentIds != null && query.Request.DepartmentIds.Any())
            q = q.Where(x => query.Request.DepartmentIds.Contains(x.Department.Id));

        if (query.Request.IsActive.HasValue)
            q = q.Where(x => x.User.IsActive == query.Request.IsActive.Value);

        if (query.Request.HireDateMin.HasValue)
            q = q.Where(x => x.Employee.HireDate >= query.Request.HireDateMin.Value);

        if (query.Request.HireDateMax.HasValue)
            q = q.Where(x => x.Employee.HireDate <= query.Request.HireDateMax.Value);

        if (query.Request.LastLoginDateMin.HasValue)
            q = q.Where(x => x.User.LastLoginAt >= query.Request.LastLoginDateMin.Value);

        if (query.Request.LastLoginDateMax.HasValue)
            q = q.Where(x => x.User.LastLoginAt <= query.Request.LastLoginDateMax.Value);

        if (query.Request.CreatedAtMin.HasValue)
            q = q.Where(x => x.User.CreatedAt >= query.Request.CreatedAtMin.Value);

        if (query.Request.CreatedAtMax.HasValue)
            q = q.Where(x => x.User.CreatedAt <= query.Request.CreatedAtMax.Value);

        if (query.Request.UserType.HasValue)
            q = q.Where(x => x.User.UserType == query.Request.UserType.Value);

        var result = await q
            .OrderBy(x => x.User.FirstName)
            .ThenBy(x => x.User.LastName)
            .ToListAsync(ct);

        var response = result
            .GroupBy(x => x.Employee.Id)
            .Select(g =>
            {
                var first = g.First();
                return new EmployeeListResponse (
                    first.Employee.Id,
                    first.User.Id,
                    first.User.FirstName,
                    first.User.LastName,
                    $"{first.User.FirstName} {first.User.LastName}",
                    first.User.Email!,
                    first.Employee.JobTitle,
                    first.User.IsActive,
                    [.. g.Where(x => x.Department != null)
                        .Select(x => new DepartmentFromEmployeeResponse(
                            x.Department.Id,
                            x.Department.Name
                        ))]

                );
            })
            .ToList();

        await auditService.LogActionAsync(
            action: AuditAction.EmployeeListed,
            module: AuditModules.Employees,
            entityName: AuditEntityNames.Employee,
            outcome: AuditOutcome.Success,
            ct: ct
            );

        return response;
    }
}