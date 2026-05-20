using Application.DTOs.Clients;

namespace Application.Features.Clients.Queries.GetAll;

public sealed record GetAllClientsQuery() : IQuery<List<ClientResponse>>;

internal sealed class GetAllClientsQueryHandler(
    IClientRepository repo,
    IAuditService auditService
    ) : IQueryHandler<GetAllClientsQuery, List<ClientResponse>>
{
    public async Task<Result<List<ClientResponse>>> HandleAsync(GetAllClientsQuery query, CancellationToken ct = default)
    {
        var entities = await repo.GetAllAsync(x => true, include: [nameof(Client.AppUser)], ct: ct);

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