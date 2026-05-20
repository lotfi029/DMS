namespace Application.Features.Departments.Queries.Get;

public sealed record GetDepartmentByIdQuery(Guid Id) : IQuery<DepartmentResponse>;
internal sealed class GetDepartmentByIdQueryHandler(
    IDepartmentRepository repo,
    IAuditService auditService
    ) : IQueryHandler<GetDepartmentByIdQuery, DepartmentResponse>
{
    public async Task<Result<DepartmentResponse>> HandleAsync(GetDepartmentByIdQuery query, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(query.Id, ct);

        if (entity is null)
            return DepartmentErrors.NotFound;

        var cnt = await repo.GetEmployeeCountAsync(entity.Id, ct);
        var departmentHeadName = await repo.GetDepartmentNameAsync(query.Id, ct);

        var response = new DepartmentResponse(
            Id: entity.Id,
            Name: entity.Name,
            Description: entity.Description,
            IsActive: entity.IsActive,
            DepartmentHeadId: entity.DepartmentHeadId,
            DepartmentHeadName: departmentHeadName,
            EmployeeCount: cnt,
            CreatedAt: entity.CreatedAt,
            UpdatedAt: DateTime.MinValue
            );

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentViewed,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: entity.Id.ToString(),
            ct: ct);

        return response;
    }
}
