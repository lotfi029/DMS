using Application.DTOs.Clients;

namespace Application.Features.Clients.Queries.GetById;

public sealed record GetClientByIdQuery(Guid Id) : IQuery<ClientResponse>;

internal sealed class GetClientByIdQueryHandler(
    IClientRepository repo,
    IAuditService auditService
    ) : IQueryHandler<GetClientByIdQuery, ClientResponse>
{
    public async Task<Result<ClientResponse>> HandleAsync(GetClientByIdQuery query, CancellationToken ct = default)
    {
        if (await repo.GetByIdAsync(c => c.Id == query.Id, include: [nameof(Client.AppUser)], ct: ct) is not { } entity)
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