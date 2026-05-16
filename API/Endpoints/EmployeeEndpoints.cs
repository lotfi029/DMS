using Application.DTOs.Employees;
using Application.Features.Employees.Commands.Create;
using Application.Features.Employees.Queries.GetAll;
using Application.Features.Employees.Queries.GetById;

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

        group.MapPost("", CreateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Create))
            .Produces<Guid>(StatusCodes.Status201Created)
            .WithName("CreateEmployee");

        group.MapPut("update", UpdateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Update))
            .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("delete", DeleteAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Delete))
            .Produces(StatusCodes.Status204NoContent);

        group.MapGet("{id:guid}", GetByIdAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.ViewDetails))
            .Produces<EmployeeListResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetEmployeeById");

        group.MapPost("get-all", GetAllAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Read))
            .Produces<IEnumerable<EmployeeListResponse>>(StatusCodes.Status200OK);

        group.MapGet("by-role/{roleName}", GetByRoleAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Read))
            .Produces<IEnumerable<EmployeeListResponse>>(StatusCodes.Status200OK);
    }

    private async Task<IResult> CreateAsync(
        [FromBody] CreateEmployeeRequest request,
        [FromServices] IValidator<CreateEmployeeRequest> validator,
        [FromServices] ICommandHandler<CreateEmployeeCommand, Guid> handler,
        CancellationToken ct
        )
    {
        var command = new CreateEmployeeCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.UserName,
            request.Password,
            request.JobTitle,
            request.RoleId,
            request.DepartmentId,
            request.GrantPermissions,
            request.DenyPermissions,
            request.Notes);

        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.CreatedAtRoute("GetEmployeeById", new { id = result.Value }, result.Value)
            : result.ToProblem();
    }
    private Task<IResult> UpdateAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    private Task<IResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
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
            : Results.Problem();
    }
    private async Task<IResult> GetAllAsync(
        [FromBody] EmployeeQueryRequest queryRequest,
        [FromServices] IQueryHandler<GetAllEmployeeQuery, IEnumerable<EmployeeListResponse>> handler,
        CancellationToken ct
        )
    {
        var query = new GetAllEmployeeQuery(
            JobTitle: queryRequest.JobTitle,
            Role: queryRequest.Role,
            DepartmentId: queryRequest.DepartmentId,
            IsActive: queryRequest.IsActive,
            HireDateMin: queryRequest.HireDateMin,
            HireDateMax: queryRequest.HireDateMax,
            LastLoginDateMin: queryRequest.LastLoginDateMin,
            LastLoginDateMax: queryRequest.LastLoginDateMax,
            CreatedAtMin: queryRequest.CreatedAtMin,
            CreatedAtMax: queryRequest.CreatedAtMax,
            UserType: queryRequest.UserType);

        var result = await handler.HandleAsync(query, ct);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.Problem();

    }
    private Task<IResult> GetByRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }
}