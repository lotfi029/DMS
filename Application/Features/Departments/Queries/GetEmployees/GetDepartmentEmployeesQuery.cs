using Application.DTOs.Employees;

namespace Application.Features.Departments.Queries.GetEmployees;

public sealed record GetDepartmentEmployeesQuery(Guid DepartmentId) : IQuery<List<EmployeeListResponse>>;

internal sealed class GetDepartmentEmployeesQueryHandler(
    IEmployeeDepartmentRepository repo,
    IAuditService auditService) : IQueryHandler<GetDepartmentEmployeesQuery, List<EmployeeListResponse>>
{
    public async Task<Result<List<EmployeeListResponse>>> HandleAsync(GetDepartmentEmployeesQuery query, CancellationToken ct = default)
    {
        var result = await repo.GetEmployeeAsync(query.DepartmentId, ct);

        if (result is null || !result.Any())
            return Result.Success((List<EmployeeListResponse>)[]);

        var response = result.GroupBy(k => k.EmployeeId).Select(g =>
        {
            var employee = g.First();
            return new EmployeeListResponse(
                Id: employee.EmployeeId,
                UserId: employee.Employee.AppUserId,
                FirstName: employee.Employee.AppUser.FirstName,
                LastName: employee.Employee.AppUser.LastName,
                FullName: $"{employee.Employee.AppUser.FirstName} {employee.Employee.AppUser.LastName}",
                Email: employee.Employee.AppUser.Email!,
                JobTitle: employee.Employee.JobTitle,
                IsActive: employee.Employee.IsActive,
                Departments: [.. g.Select(d => new DepartmentFromEmployeeResponse(
                    Id: d.DepartmentId,
                    Name: d.Department.Name
                ))]
            );
        }).ToList();

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentUserViewed,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: query.DepartmentId.ToString(),
            ct: ct);

        return response;
    }
}