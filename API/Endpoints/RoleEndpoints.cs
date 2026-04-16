using Application.Features.Roles.Commands.Create;
using Application.Features.Roles.Commands.Delete;
using Application.Features.Roles.Commands.Update;
using Application.Features.Roles.Commands.AssignToUser;
using Application.Features.Roles.Commands.RemoveFromUser;
using Application.Features.Roles.Queries.GetUserRoles;
using Application.Features.Roles.Queries.GetAll;
using Application.DTOs.Roles;

namespace API.Endpoints;

internal sealed class RoleEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/roles")
            .WithTags("Roles")
            .RequireAuthorization();

        group.MapPost("/create", CreateRoleAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Roles.Create));
        group.MapPost("/assign-to-user", AssignRoleToUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Roles.AssignToUser));
        group.MapPost("/remove-from-user", RemoveRoleFromUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Roles.RemoveFromUser));

        group.MapPut("/update", UpdateRoleAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Roles.Update));

        group.MapDelete("/{roleId}", DeleteRoleAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Roles.Delete));

        group.MapGet("/user/{userId}", GetUserRolesAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Roles.Read));
        group.MapGet("/", GetAllRolesAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Roles.Read));
    }

    private async Task<IResult> CreateRoleAsync(
        [FromBody] CreateRoleCommand command,
        [FromServices] ICommandHandler<CreateRoleCommand> handler,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess ? Results.Ok() : result.ToProblem();
    }

    private async Task<IResult> DeleteRoleAsync(
        [FromRoute] string roleId,
        [FromServices] ICommandHandler<DeleteRoleCommand> handler,
        CancellationToken ct)
    {
        var command = new DeleteRoleCommand(roleId);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess ? Results.Ok() : result.ToProblem();
    }

    private async Task<IResult> UpdateRoleAsync(
        [FromBody] UpdateRoleCommand command,
        [FromServices] ICommandHandler<UpdateRoleCommand> handler,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess ? Results.Ok() : result.ToProblem();
    }

    private async Task<IResult> AssignRoleToUserAsync(
        [FromBody] AssignRoleToUserCommand command,
        [FromServices] ICommandHandler<AssignRoleToUserCommand> handler,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess ? Results.Ok() : result.ToProblem();
    }

    private async Task<IResult> RemoveRoleFromUserAsync(
        [FromBody] RemoveRoleFromUserCommand command,
        [FromServices] ICommandHandler<RemoveRoleFromUserCommand> handler,
        CancellationToken ct)
    {
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess ? Results.Ok() : result.ToProblem();
    }

    private async Task<IResult> GetUserRolesAsync(
        [FromRoute] string userId,
        [FromServices] IQueryHandler<GetUserRolesQuery, IEnumerable<RoleResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetUserRolesQuery(userId);
        var result = await handler.HandleAsync(query, ct);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblem();
    }

    private async Task<IResult> GetAllRolesAsync(
        [FromServices] IQueryHandler<GetAllRolesQuery, IEnumerable<RoleResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetAllRolesQuery();
        var result = await handler.HandleAsync(query, ct);
        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblem();
    }
}
