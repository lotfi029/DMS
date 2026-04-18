namespace Application.Features.Departments.Queries.Get;

public sealed record GetDepartmentByIdQuery(Guid Id) : IQuery<DepartmentResponse>;

public sealed class GetDepartmentByIdQueryHandler(
    IDepartmentDomainService domainService,
    IAuditService auditService
    ) : IQueryHandler<GetDepartmentByIdQuery, DepartmentResponse>
{
    public async Task<Result<DepartmentResponse>> HandleAsync(GetDepartmentByIdQuery query, CancellationToken ct = default)
    {
        var entity = await domainService.GetByIdAsync(query.Id, ct);

        if (entity.IsFailure)
            return entity.Error;

        var reponse = entity.Value!.Adapt<DepartmentResponse>();
        
        await auditService.LogActionAsync(
            action: AuditAction.DepartmentViewed,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: entity.Value!.Id.ToString(),
            ct: ct);

        return reponse;
    }
}
