namespace Application.Features.Departments.Queries.Get;

public sealed record GetDepartmentByIdQuery(Guid Id) : IQuery<DepartmentResponse>;

public sealed class GetDepartmentByIdQueryHandler(
    IDepartmentRepository repo,
    IAuditService auditService
    ) : IQueryHandler<GetDepartmentByIdQuery, DepartmentResponse>
{
    public async Task<Result<DepartmentResponse>> HandleAsync(GetDepartmentByIdQuery query, CancellationToken ct = default)
    {
        var entity = await repo.GetByIdAsync(query.Id, ct);

        if (entity is null)
            return DepartmentErrors.NotFound;

        var reponse = entity.Adapt<DepartmentResponse>();

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentViewed,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: entity.Id.ToString(),
            ct: ct);

        return reponse;
    }
}
