using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Departments.Queries.Get.GetUsers;

public sealed record GetDepartmentUsersQuery(Guid DepartmentId) : IQuery<List<UserListResponse>>;

internal sealed class GetAllUsersQueryHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService) : IQueryHandler<GetDepartmentUsersQuery, List<UserListResponse>>
{
    public async Task<Result<List<UserListResponse>>> HandleAsync(GetDepartmentUsersQuery query, CancellationToken ct = default)
    {
        var result = await departmentDomainService.GetUsersAsync(u => u.DepartmentId == query.DepartmentId && u.IsActive, ct);
        
        if (result.IsFailure)
            return result.Error;

        var response = result.Value!.Adapt<List<UserListResponse>>();
        
        await auditService.LogActionAsync(
            action: AuditAction.DepartmentUserViewed,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: query.DepartmentId.ToString(),
            ct: ct);

        return response;
    }
}