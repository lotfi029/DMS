namespace Application.Features.Departments.Queries.GetAll;

public sealed record GetAllDepartmentsQuery : IQuery<IEnumerable<DepartmentResponse>>;

public sealed class GetAllDepartmentsQueryHandler(
    IDepartmentDomainService domainService,
    IAuditService auditService
    ) : IQueryHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentResponse>>
{
    public async Task<Result<IEnumerable<DepartmentResponse>>> HandleAsync(GetAllDepartmentsQuery query, CancellationToken ct = default)
    {
        var entities = await domainService.GetAllAsync(ct);

        if (entities.IsFailure)
            return entities.Error;

        var responses = entities.Value!.Adapt<IEnumerable<DepartmentResponse>>();
        
        await auditService.LogActionAsync(
            action: AuditAction.DepartmentListed,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            ct: ct);

        return Result.Success(responses);
    }
}