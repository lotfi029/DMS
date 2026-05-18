using Application.DTOs.Employees;

namespace Application.Features.Departments.Queries.Get.GetUsers;

public sealed record GetDepartmentEmployeesQuery(Guid DepartmentId) : IQuery<List<EmployeeListResponse>>;

internal sealed class GetDepartmentEmployeesQueryHandler(
    IEmployeeDepartmentRepository employeeDepartmentRepository,
    IAuditService auditService) : IQueryHandler<GetDepartmentEmployeesQuery, List<EmployeeListResponse>>
{
    public async Task<Result<List<EmployeeListResponse>>> HandleAsync(GetDepartmentEmployeesQuery query, CancellationToken ct = default)
    {
        var result = await employeeDepartmentRepository.GetEmployeeAsync(query.DepartmentId, ct);

        if (result is null || !result.Any())
            return Result.Success((List<EmployeeListResponse>)[]);

        var response = result.Select(e => new EmployeeListResponse(
            e.Employee.Id,
            e.Employee.AppUser.Id,
            e.Employee.AppUser.FirstName,
            e.Employee.AppUser.LastName,
            e.Employee.AppUser.Email!,
            e.Employee.JobTitle,
            e.Employee.IsActive,
            e.DepartmentId,
            null
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