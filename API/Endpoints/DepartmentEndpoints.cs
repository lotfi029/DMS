using Application.DTOs.Departments;
using Application.Features.Departments.Commands.Activate;
using Application.Features.Departments.Commands.AssignHead;
using Application.Features.Departments.Commands.Create;
using Application.Features.Departments.Commands.Deactivate;
using Application.Features.Departments.Commands.Delete;
using Application.Features.Departments.Commands.UnAssignHead;
using Application.Features.Departments.Commands.Update;
using Application.Features.Departments.Queries.Get;
using Application.Features.Departments.Queries.GetAll;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.IdentityModel.Tokens;

namespace API.Endpoints;

internal sealed class DepartmentEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/departments")
            .WithTags("Departments")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/create", CreateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.Create))
            .Produces(StatusCodes.Status201Created);

        group.MapPut("/{id:guid}/assign-head", AssignHeadAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.AssignDepartmentHead))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/unassign-head", UnAssignHeadAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.UnassignDepartmentHead))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/activate", ActiveAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.Update))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/deactivate", InActiveAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.Update))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/update", UpdateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.Update))
            .Produces(StatusCodes.Status204NoContent);
        
        group.MapDelete("/{id:guid}", DeleteAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.Delete))
            .Produces(StatusCodes.Status204NoContent);
        
        group.MapGet("/{id:guid}", GetByIdAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.Read))
            .ProducesProblem(StatusCodes.Status404NotFound)
            .Produces<DepartmentResponse>(StatusCodes.Status200OK);
        group.MapGet("/", GetAllAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.Read))
            .Produces<IEnumerable<DepartmentResponse>>(StatusCodes.Status200OK);
    }
    
    private async Task<IResult> CreateAsync(
        [FromBody] CreateDepartmentRequest request,
        [FromServices] ICommandHandler<CreateDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new CreateDepartmentCommand(request);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess 
            ? Results.Created() 
            : result.ToProblem();
    }

    private async Task<IResult> AssignHeadAsync(
        [FromRoute] Guid id,
        [FromBody] DepartmentEmployeeRequest request,
        [FromServices] ICommandHandler<AssignDepartmentHeadCommand> handler,
        CancellationToken ct)
    {
        var command = new AssignDepartmentHeadCommand(id, request.EmployeeId);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> UnAssignHeadAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<UnAssignDepartmentHeadCommand> handler,
        CancellationToken ct)
    {
        var command = new UnAssignDepartmentHeadCommand(id);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateDepartmentRequest request,
        [FromServices] ICommandHandler<UpdateDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new UpdateDepartmentCommand(id, request);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> ActiveAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<ActiveDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new ActiveDepartmentCommand(id);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> InActiveAsync(
        [FromRoute] Guid id,
        [FromQuery] Guid? newDepartmentId,
        [FromServices] ICommandHandler<DeactivateDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new DeactivateDepartmentCommand(id, newDepartmentId);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> DeleteAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<DeleteDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new DeleteDepartmentCommand(id);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess 
            ? Results.Ok() 
            : result.ToProblem();
    }

    private async Task<IResult> GetByIdAsync(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetDepartmentByIdQuery, DepartmentResponse> handler,
        CancellationToken ct)
    {
        var query = new GetDepartmentByIdQuery(id);
        var result = await handler.HandleAsync(query, ct);
        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : result.ToProblem();
    }

    private async Task<IResult> GetAllAsync(
        [FromServices] IQueryHandler<GetAllDepartmentsQuery, IEnumerable<DepartmentResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetAllDepartmentsQuery();
        var result = await handler.HandleAsync(query, ct);
        return result.IsSuccess 
            ? Results.Ok(result.Value) 
            : result.ToProblem();
    }
}