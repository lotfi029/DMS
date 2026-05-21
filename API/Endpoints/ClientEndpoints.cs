using Application.DTOs.Clients;
using Application.Features.Clients.Commands.Activate;
using Application.Features.Clients.Commands.Create;
using Application.Features.Clients.Commands.Deactivate;
using Application.Features.Clients.Commands.Delete;
using Application.Features.Clients.Commands.Update;
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
        group.MapPut("/{id:guid}", UpdateClientAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Clients.Update))
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPut("/{id:guid}/activate", ActivateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Clients.Update))
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapPut("/{id:guid}/deactivate", DeactivateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Clients.Update))
            .ProducesProblem(StatusCodes.Status404NotFound);
        group.MapDelete("/{id:guid}", DeleteAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Clients.Delete))
            .ProducesProblem(StatusCodes.Status404NotFound);

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
    private async Task<IResult> UpdateClientAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateClientRequest request,
        [FromServices] ICommandHandler<UpdateClientCommand> handler,
        CancellationToken ct)
    {
        var command = new UpdateClientCommand(id, request);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> ActivateAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<ActivateClientCommand> handler,
        CancellationToken ct)
    {
        var command = new ActivateClientCommand(id);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> DeactivateAsync(
        [FromRoute] Guid id,
        [FromQuery] string userId,
        [FromServices] ICommandHandler<DeactivateClientCommand> handler,
        CancellationToken ct)
    {
        var command = new DeactivateClientCommand(id, userId);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> DeleteAsync(
        [FromRoute] Guid id,
        [FromQuery] string userId,
        [FromServices] ICommandHandler<DeleteClientCommand> handler,
        CancellationToken ct)
    {
        var command = new DeleteClientCommand(id, userId
            );
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
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