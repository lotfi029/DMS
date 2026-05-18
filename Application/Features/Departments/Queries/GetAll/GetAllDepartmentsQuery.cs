namespace Application.Features.Departments.Queries.GetAll;

public sealed record GetAllDepartmentsQuery : IQuery<IEnumerable<DepartmentResponse>>;

public sealed class GetAllDepartmentsQueryHandler(
    IDepartmentRepository repo,
    IAuditService auditService
    ) : IQueryHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentResponse>>
{
    public async Task<Result<IEnumerable<DepartmentResponse>>> HandleAsync(GetAllDepartmentsQuery query, CancellationToken ct = default)
    {
        var entities = await repo.GetAllAsync(ct);

        if (entities is null || !entities.Any())
            return Result.Success(Enumerable.Empty<DepartmentResponse>());


        var responses = entities.Adapt<IEnumerable<DepartmentResponse>>();

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentListed,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            ct: ct);

        return Result.Success(responses);
    }
}