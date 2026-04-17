using Application.DTOs.Auths;
using Application.DTOs.Users;
using Application.Features.Users.Commands.Activate;
using Application.Features.Users.Commands.Create;
using Application.Features.Users.Commands.Deactivate;
using Application.Features.Users.Commands.Delete;
using Application.Features.Users.Commands.Update;
using Application.Features.Users.Queries.GetAll;
using Application.Features.Users.Queries.GetById;

namespace API.Endpoints; 

internal sealed class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Users")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/create", CreateUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Users.Create))
            .Produces<CreateUserResponse>(StatusCodes.Status201Created);

        group.MapPost("/{id}/deactivate", DeactivateUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Users.Deactivate))
            .Produces(StatusCodes.Status204NoContent);

        group.MapPost("/{id}/activate", ActivateUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Users.Update))
            .Produces(StatusCodes.Status204NoContent);
        
        group.MapPut("/{id:guid}/update", UpdateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Users.Update))
            .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("/{id}", DeleteUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Users.Delete))
            .Produces(StatusCodes.Status204NoContent);

        group.MapGet("/{id}", GetUserByIdAsync)
            .WithName("GetUserById")
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Users.Read))
            .Produces<DetailedUserResponse>(StatusCodes.Status200OK);

        group.MapGet("/me", Profile)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Users.ViewProfile))
            .Produces<DetailedUserResponse>(StatusCodes.Status200OK);

        group.MapGet("/", GetAllUsersAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Users.Read))
            .Produces<List<UserListResponse>>(StatusCodes.Status200OK);
    }

    private async Task<IResult> CreateUserAsync(
        [FromBody] AddUserRequest request,
        [FromServices] IValidator<AddUserRequest> validator,
        [FromServices] ICommandHandler<CreateUserCommand, CreateUserResponse> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var command = new CreateUserCommand(request);
        var result = await handler.HandleAsync(command, ct);
        
        return result.IsSuccess 
            ? Results.CreatedAtRoute("GetUserById", new { id = result.Value!.UserId }, result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> UpdateAsync(
        [FromRoute] string id,
        [FromBody] UpdateUserRequest request,
        [FromServices] IValidator<UpdateUserRequest> validator,
        [FromServices] ICommandHandler<UpdateUserCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var command = new UpdateUserCommand(id, request);
        var result = await handler.HandleAsync(command, ct);
        
        return result.IsSuccess 
            ? Results.NoContent() 
            : result.ToProblem();
    }
    private async Task<IResult> DeactivateUserAsync(
        [FromRoute] string id,
        [FromServices] ICommandHandler<DeactivateUserCommand> handler,
        CancellationToken ct)
    {
        var command = new DeactivateUserCommand(id);
        var result = await handler.HandleAsync(command, ct);
        
        return result.IsSuccess 
            ? Results.NoContent()
            : result.ToProblem();
    }

    private async Task<IResult> ActivateUserAsync(
        [FromRoute] string id,
        [FromServices] ICommandHandler<ActivateUserCommand> handler,
        CancellationToken ct)
    {
        var command = new ActivateUserCommand(id);
        var result = await handler.HandleAsync(command, ct);
        
        return result.IsSuccess 
            ? Results.NoContent() 
            : result.ToProblem();
    }

    private async Task<IResult> DeleteUserAsync(
        [FromRoute] string id,
        [FromServices] ICommandHandler<DeleteUserCommand> handler,
        CancellationToken ct)
    {
        var command = new DeleteUserCommand(id);
        var result = await handler.HandleAsync(command, ct);
        
        return result.IsSuccess 
            ? Results.NoContent() 
            : result.ToProblem();
    }
    private async Task<IResult> Profile(
        HttpContext httpContext,
        [FromServices] IQueryHandler<GetUserByIdQuery, DetailedUserResponse> handler,
        CancellationToken ct = default)
    {
        var userId = httpContext.GetUserId();
        var command = new GetUserByIdQuery(userId);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetUserByIdAsync(
      [FromRoute] string id,
      [FromServices] IQueryHandler<GetUserByIdQuery, DetailedUserResponse> handler,
      CancellationToken ct)
    {
        var query = new GetUserByIdQuery(id);
        var result = await handler.HandleAsync(query, ct);
        
        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : result.ToProblem();
    }

    private async Task<IResult> GetAllUsersAsync(
        [FromServices] IQueryHandler<GetAllUsersQuery, IEnumerable<UserListResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.GetUserId();

        var query = new GetAllUsersQuery(userId);
        var result = await handler.HandleAsync(query, ct);
        
        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : result.ToProblem();
    }
}