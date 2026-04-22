using Application.DTOs.Employees;

namespace Application.Features.Departments.Queries.Get.GetUsers;

public sealed record GetDepartmentEmployeesQuery(Guid DepartmentId) : IQuery<List<EmployeeResponse>>;

internal sealed class GetDepartmentEmployeesQueryHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService) : IQueryHandler<GetDepartmentEmployeesQuery, List<EmployeeResponse>>
{
    public async Task<Result<List<EmployeeResponse>>> HandleAsync(GetDepartmentEmployeesQuery query, CancellationToken ct = default)
    {
        var result = await departmentDomainService.GetUsersAsync(u => u.DepartmentId == query.DepartmentId, ct);

        if (result.IsFailure)
            return result.Error;

        var response = result.Value!.Select(e => new EmployeeResponse(
            e.Id,
            e.AppUser.Id,
            e.AppUser.FirstName,
            e.AppUser.LastName,
            e.AppUser.Email!,
            e.JobTitle,
            e.IsActive,
            e.DepartmentId ?? null,
            e.Department!.Name ?? null
            ))
            .ToList();

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentUserViewed,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: query.DepartmentId.ToString(),
            ct: ct);

        return response;
    }
}