using Application.DTOs.Clients;
using Application.Features.Clients.Commands.Create;
using Application.Features.Clients.Queries.GetAll;
using Application.Features.Clients.Queries.GetById;

namespace API.Endpoints;

internal sealed class ClientEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/clients")
            .WithTags("Clients")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/", AddClientAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Clients.Create))
            .Produces<Guid>(StatusCodes.Status201Created);

        group.MapGet("/{id:guid}", GetByIdAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Clients.Read))
            .ProducesProblem(StatusCodes.Status404NotFound)
            .Produces<ClientResponse>(StatusCodes.Status200OK);

        group.MapGet("/", GetAllAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Clients.Read))
            .Produces<List<ClientResponse>>(StatusCodes.Status200OK);

    }

    private async Task<IResult> AddClientAsync(
        [FromBody] CreateClientRequest request,
        [FromServices] ICommandHandler<CreateClientCommand, Guid> handler,
        HttpContext context,
        CancellationToken ct)
    {
        var command = new CreateClientCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.UserName,
            request.Password,
            request.Phone,
            request.Address,
            request.Notes
            );
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.Created($"/api/clients/{result.Value}", result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetByIdAsync(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetClientByIdQuery, ClientResponse> handler,
        CancellationToken ct)
    {
        var query = new GetClientByIdQuery(id);
        var result = await handler.HandleAsync(query, ct);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetAllAsync(
        [FromServices] IQueryHandler<GetAllClientsQuery, List<ClientResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetAllClientsQuery();
        var result = await handler.HandleAsync(query, ct);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblem();
    }
}