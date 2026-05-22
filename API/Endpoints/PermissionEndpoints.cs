using Application.DTOs.Permissions;
using Application.Features.Permissions.Commands.AssignToRole;
using Application.Features.Permissions.Commands.DenyOverride;
using Application.Features.Permissions.Commands.GrantOverride;
using Application.Features.Permissions.Commands.RemoveFromRole;
using Application.Features.Permissions.Commands.RevokeOverride;
using Application.Features.Permissions.Queries.GetAll;
using Application.Features.Permissions.Queries.GetEffective;
using Application.Features.Permissions.Queries.GetRolePermissions;


namespace API.Endpoints;

internal sealed class PermissionEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/permissions")
            .WithTags("Permission Overrides")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/grant", GrantAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.Grant))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPost("/deny", DenyAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.Deny))
            .Produces(StatusCodes.Status204NoContent);
        group.MapDelete("{userId:guid}/overrides/{permission:alpha}", RevokeAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.Revoke))
            .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/{roleId:guid}", AssignPermissionToRoleAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.AssignToRole))
            .Produces(StatusCodes.Status204NoContent);
        group.MapDelete("/{roleId:guid}/{permission:alpha}", RemovePermissionFromRoleAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.RemoveFromRole))
            .Produces(StatusCodes.Status204NoContent);

        group.MapGet("/user/{userId:guid}", GetUserPermissionsAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.Read))
            .Produces<IEnumerable<PermissionResponse>>(StatusCodes.Status200OK);
        group.MapGet("/", GetAllPermissionsAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.Read))
            .Produces<IEnumerable<PermissionResponse>>(StatusCodes.Status200OK);
        group.MapGet("/role/{roleId:guid}", GetRolePermissionsAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.Read))
            .Produces<IEnumerable<string>>(StatusCodes.Status200OK);
        group.MapGet("/me", GetMyEffectivePermissionsAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Permissions.Read))
            .Produces<IEnumerable<PermissionResponse>>(StatusCodes.Status200OK);
    }
    private async Task<IResult> GrantAsync(
        [FromBody] GrantPermissionRequest request,
        [FromServices] ICommandHandler<GrantPermissionOverrideCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var callerUserId = httpContext.GetUserId();
        var command = new GrantPermissionOverrideCommand(
            request.TargetUserId,
            request.Permission,
            callerUserId,
            request.Reason,
            request.ExpiresAt
        );
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> DenyAsync(
        [FromBody] DenyPermissionRequest request,
        [FromServices] ICommandHandler<DenyPermissionOverrideCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var callerUserId = httpContext.GetUserId();
        var command = new DenyPermissionOverrideCommand(
            request.TargetUserId,
            request.Permission,
            callerUserId,
            request.Reason
        );
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> RevokeAsync(
        [FromRoute] string userId,
        [FromRoute] string permission,
        [FromServices] ICommandHandler<RevokePermissionOverrideCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var callerUserId = httpContext.GetUserId();
        var command = new RevokePermissionOverrideCommand(
            userId,
            permission,
            callerUserId
        );
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> AssignPermissionToRoleAsync(
        [FromRoute] string roleId,
        [FromBody] PermissionRequest request,
        [FromServices] ICommandHandler<AssignPermissionToRoleCommand> handler,
        CancellationToken ct)
    {
        var command = new AssignPermissionToRoleCommand(roleId, request.PermissionName);
        var result = await handler.HandleAsync(command, ct);
        
        return result.IsSuccess 
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> GetUserPermissionsAsync(
        [FromRoute] string userId,
        [FromServices] IQueryHandler<GetUserEffectivePermissionsQuery, EffectivePermissionsResponse> handler,
        CancellationToken ct)
    {
        var query = new GetUserEffectivePermissionsQuery(userId);
        var result = await handler.HandleAsync(query, ct);
        
        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : result.ToProblem();
    }
    private async Task<IResult> GetMyEffectivePermissionsAsync(
        [FromServices] IQueryHandler<GetUserEffectivePermissionsQuery, EffectivePermissionsResponse> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();
        var query = new GetUserEffectivePermissionsQuery(userId);
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

    private async Task<IResult> RemovePermissionFromRoleAsync(
        [FromRoute] string roleId,
        [FromRoute] string permission,
        [FromServices] ICommandHandler<RemovePermissionFromRoleCommand> handler,
        CancellationToken ct)
    {
        var command = new RemovePermissionFromRoleCommand(roleId, permission);
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
