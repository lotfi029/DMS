using Application.Abstractions.Data;
using Application.Abstractions.Pagination;
using Application.DTOs.Employees;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Employees.Queries.GetAll;

public sealed record GetAllEmployeeQuery(
    EmployeeQueryRequest Request) : IQuery<PagedResult<EmployeeListResponse>>;

internal sealed class GetAllEmployeeQueryHandler(
    IApplicationDbContext dbContext,
    IAuditService auditService) : IQueryHandler<GetAllEmployeeQuery, PagedResult<EmployeeListResponse>>
{
    public async Task<Result<PagedResult<EmployeeListResponse>>> HandleAsync(GetAllEmployeeQuery query, CancellationToken ct = default)
    {
        var q = dbContext.EmployeeProfiles.AsNoTracking();

        if (!string.IsNullOrEmpty(query.Request.JobTitle))
            q = q.Where(x => x.JobTitle.Contains(query.Request.JobTitle));

        if (query.Request.RoleIds != null && query.Request.RoleIds.Any())
            q = q.Where(x => dbContext.UserRoles.Any(ur => ur.UserId == x.UserId && query.Request.RoleIds.Contains(ur.RoleId)));

        if (query.Request.DepartmentIds != null && query.Request.DepartmentIds.Any())
            q = q.Where(x => query.Request.DepartmentIds.Contains(x.DepartmentId ?? Guid.Empty));

        if (query.Request.IsActive.HasValue)
            q = q.Where(x => x.EmployeeIsActive == query.Request.IsActive.Value);

        if (query.Request.HireDateMin.HasValue)
            q = q.Where(x => x.HireDate >= query.Request.HireDateMin.Value);

        if (query.Request.HireDateMax.HasValue)
            q = q.Where(x => x.HireDate <= query.Request.HireDateMax.Value);

        if (query.Request.LastLoginDateMin.HasValue)
            q = q.Where(x => x.LastLoginAt >= query.Request.LastLoginDateMin.Value);

        if (query.Request.LastLoginDateMax.HasValue)
            q = q.Where(x => x.LastLoginAt <= query.Request.LastLoginDateMax.Value);

        if (query.Request.CreatedAtMin.HasValue)
            q = q.Where(x => x.CreatedAt >= query.Request.CreatedAtMin.Value);

        if (query.Request.CreatedAtMax.HasValue)
            q = q.Where(x => x.CreatedAt <= query.Request.CreatedAtMax.Value);


        var data = await q
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(ct);

        var items = data
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
            .Select(g => new EmployeeListResponse(
                g.Key.EmployeeId,
                g.Key.UserId,
                g.Key.FirstName,
                g.Key.LastName,
                FullName: $"{g.Key.FirstName} {g.Key.LastName}",
                g.Key.Email,
                g.Key.JobTitle,
                g.Key.EmployeeIsActive,

                [.. g.Where(x => x.DepartmentId != null)
                 .Select(x => new DepartmentFromEmployeeResponse(
                     x.DepartmentId!.Value,
                     x.DepartmentName!
                 ))
                 .Distinct()],

                [.. g.Where(x => x.UserRoleId != null)
                 .Select(x => new RoleForEmployeeResponse(
                     x.UserRoleId!,
                     x.RoleName!
                 ))
                 .Distinct()]
            ))
            .Skip(query.Request.PageSize * (query.Request.Page - 1))
            .Take(query.Request.PageSize)
            .ToList();

        var totalCount = items.Count;

        await auditService.LogActionAsync(
            action: AuditAction.EmployeeListed,
            module: AuditModules.Employees,
            entityName: AuditEntityNames.Employee,
            outcome: AuditOutcome.Success,
            ct: ct
            );

        return new PagedResult<EmployeeListResponse>(
            items, query.Request.Page, totalCount, query.Request.PageSize);
    }
}