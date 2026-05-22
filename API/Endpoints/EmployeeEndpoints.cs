using Application.Abstractions.Pagination;
using Application.DTOs.Employees;
using Application.Features.Employees.Commands.Active;
using Application.Features.Employees.Commands.Create;
using Application.Features.Employees.Commands.Deactivate;
using Application.Features.Employees.Commands.Update;
using Application.Features.Employees.Queries.GetAll;
using Application.Features.Employees.Queries.GetById;
using Application.Features.Employees.Queries.GetByRoleId;

namespace API.Endpoints;

internal sealed class EmployeeEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/employees")
            .WithTags("Employees")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/", CreateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Create))
            .Produces<Guid>(StatusCodes.Status201Created)
            .WithName("CreateEmployee");

        group.MapPut("/{id:guid}", UpdateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Update))
            .Produces(StatusCodes.Status204NoContent);
        
        group.MapPut("/{id:guid}/active", ActivateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Update))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/deactive", InactivateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Update))
            .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("{id:guid}/delete", DeleteAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Delete))
            .Produces(StatusCodes.Status204NoContent);

        group.MapGet("/{id:guid}", GetByIdAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.ViewDetails))
            .Produces<EmployeeListResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetEmployeeById");
        group.MapGet("/get-all", GetAllAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Read))
            .Produces<PagedResult<EmployeeListResponse>>(StatusCodes.Status200OK);
        group.MapGet("/by-role/{roleId:guid}", GetByRoleAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Read))
            .Produces<PagedResult<EmployeeListResponse>>(StatusCodes.Status200OK);
    }

    private async Task<IResult> CreateAsync(
        [FromBody] CreateEmployeeRequest request,
        [FromServices] ICommandHandler<CreateEmployeeCommand, Guid> handler,
        CancellationToken ct
        )
    {
        var command = new CreateEmployeeCommand(request);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.CreatedAtRoute("GetEmployeeById", new { id = result.Value }, result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateEmployeeRequest request,
        [FromServices] ICommandHandler<UpdateEmployeeCommand> handler,
        CancellationToken ct
        )
    {
        var command = new UpdateEmployeeCommand(id, request);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    

    private async Task<IResult> ActivateAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<ActivateEmployeeCommand> handler,
        CancellationToken ct
        )
    {
        var command = new ActivateEmployeeCommand(id);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> InactivateAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<DeactivateEmployeeCommand> handler,
        CancellationToken ct
        )
    {
        var command = new DeactivateEmployeeCommand(id);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> DeleteAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<DeactivateEmployeeCommand> handler,
        CancellationToken ct
        )
    {
        var command = new DeactivateEmployeeCommand(id);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> GetByIdAsync(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetEmployeeByIdQuery, EmployeeResponse> handler,
        CancellationToken ct
        )
    {
        if (id == Guid.Empty)
            return Results.BadRequest("Invalid Id");

        var query = new GetEmployeeByIdQuery(id);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetAllAsync(
        [AsParameters] EmployeeQueryRequest queryRequest,
        [FromServices] IQueryHandler<GetAllEmployeeQuery, PagedResult<EmployeeListResponse>> handler,
        CancellationToken ct
        )
    {
        var query = new GetAllEmployeeQuery(queryRequest);
        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.Problem();

    }
    private async Task<IResult> GetByRoleAsync(
        [FromRoute] string roleId,
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromServices] IQueryHandler<GetEmployeeGetByRoleIdQuery, PagedResult<EmployeeListResponse>> handler,
        CancellationToken ct
        )
    {
        var command = new GetEmployeeGetByRoleIdQuery(roleId, pageNumber, pageSize);
        
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.Problem();
    }
}