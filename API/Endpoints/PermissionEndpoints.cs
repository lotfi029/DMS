using Application.Features.Permissions.Queries.GetUserPermissions;
using Application.Features.Permissions.Queries.GetAll;
using Application.Features.Permissions.Commands.AssignToRole;
using Application.Features.Permissions.Commands.RemoveFromRole;
using Application.Features.Permissions.Queries.GetRolePermissions;
using Application.DTOs.Permissions;

namespace API.Endpoints;

internal sealed class PermissionEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/permissions")
            .WithTags("Permissions")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("{roleId:guid}/create", CreatePermissionAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.AssignToRole))
            .Produces(StatusCodes.Status204NoContent);
        group.MapDelete("{roleId:guid}/delete", DeletePermissionAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.RemoveFromRole))
            .Produces(StatusCodes.Status204NoContent);

        group.MapGet("/user/{userId}", GetUserPermissionsAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.Read))
            .Produces<IEnumerable<PermissionResponse>>(StatusCodes.Status200OK);
        group.MapGet("/", GetAllPermissionsAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.Read))
            .Produces<IEnumerable<PermissionResponse>>(StatusCodes.Status200OK);
        group.MapGet("/role/{roleId}", GetRolePermissionsAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.Read))
            .Produces<IEnumerable<string>>(StatusCodes.Status200OK);
    }

    private async Task<IResult> CreatePermissionAsync(
        [FromRoute] string roleId,
        [FromBody] PermissionRequest request,
        [FromServices] IValidator<PermissionRequest> validator,
        [FromServices] ICommandHandler<AssignPermissionToRoleCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var command = new AssignPermissionToRoleCommand(roleId, request.PermissionName);
        var result = await handler.HandleAsync(command, ct);
        
        return result.IsSuccess 
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> GetUserPermissionsAsync(
        [FromRoute] string userId,
        [FromServices] IQueryHandler<GetUserPermissionsQuery, IEnumerable<PermissionResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetUserPermissionsQuery(userId);
        var result = await handler.HandleAsync(query, ct);
        
        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : result.ToProblem();
    }

    private async Task<IResult> GetAllPermissionsAsync(
        [FromServices] IQueryHandler<GetAllPermissionsQuery, IEnumerable<PermissionResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetAllPermissionsQuery();
        var result = await handler.HandleAsync(query, ct);
        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : result.ToProblem();
    }

    private async Task<IResult> DeletePermissionAsync(
        [FromRoute] string roleId,
        [FromBody] PermissionRequest request,
        [FromServices] IValidator<PermissionRequest> validator,
        [FromServices] ICommandHandler<RemovePermissionFromRoleCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());
        
        var command = new RemovePermissionFromRoleCommand(roleId, request.PermissionName);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess 
            ? Results.Ok() 
            : result.ToProblem();
    }
    private async Task<IResult> GetRolePermissionsAsync(
        [FromRoute] string roleId,
        [FromServices] IQueryHandler<GetRolePermissionsQuery, IEnumerable<PermissionResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetRolePermissionsQuery(roleId);
        var result = await handler.HandleAsync(query, ct);
        
        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : result.ToProblem();
    }
}
