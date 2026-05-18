using Application.DTOs.Clients;

namespace Application.Features.Clients.Queries;

public sealed record GetClientByIdQuery(Guid Id) : IQuery<ClientResponse>;


internal sealed class GetClientByIdQueryHandler(
    IClientRepository repo,
    IAuditService auditService
    ) : IQueryHandler<GetClientByIdQuery, ClientResponse>
{
    public async Task<Result<ClientResponse>> HandleAsync(GetClientByIdQuery query, CancellationToken ct = default)
    {
        if (repo.GetByIdAsync(c => c.Id == query.Id, [nameof(Client.AppUser)], ct) is not { } entity)
            return ClientErrors.NotFound;

        var reponse = entity.Adapt<ClientResponse>();

        await auditService.LogActionAsync(
            action: AuditAction.ClientViewed,
            module: AuditModules.Clients,
            entityName: AuditEntityNames.Client,
            entityId: entity.Id.ToString(),
            ct: ct);
        return reponse;
    }
}
public sealed record GetAllClientsQuery() : IQuery<List<ClientResponse>>;

internal sealed class GetAllClientsQueryHandler(
    IClientRepository repo,
    IAuditService auditService
    ) : IQueryHandler<GetAllClientsQuery, List<ClientResponse>>
{
    public async Task<Result<List<ClientResponse>>> HandleAsync(GetAllClientsQuery query, CancellationToken ct = default)
    {
        var entities = await repo.GetAllAsync(x => true, [nameof(Client.AppUser)], ct);

        if (entities is null || !entities.Any())
            return Result.Success(Enumerable.Empty<ClientResponse>().ToList());

        var responses = entities.Adapt<IEnumerable<ClientResponse>>().ToList();

        await auditService.LogActionAsync(
            action: AuditAction.ClientListed,
            module: AuditModules.Clients,
            entityName: AuditEntityNames.Client,
            ct: ct);
        return Result.Success(responses);
    }
}