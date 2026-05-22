using Application.Abstractions.Pagination;
using Application.DTOs.Departments;
using Application.DTOs.Employees;
using Application.Features.EmployeeDepartments.Commands.AddEmployee;
using Application.Features.EmployeeDepartments.Commands.MoveUser;
using Application.Features.EmployeeDepartments.Commands.RemoveUser;
using Application.Features.EmployeeDepartments.Queries.GetDepartments;
using Application.Features.EmployeeDepartments.Queries.GetEmployees;

namespace API.Endpoints;

internal sealed class EmployeeDepartmentEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/employee-departments")
            .WithTags("Employee Departments")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/{id:guid}/add-employee", AddEmployeeAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.AssignToUser))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/remove-employee", RemoveEmployeeAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.RemoveFromUser))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/move-employee", MoveEmployeeAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.MoveUser))
            .Produces(StatusCodes.Status204NoContent);

        group.MapGet("/{id:guid}/employees", GetEmployeesAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.ViewUsers))
            .Produces<IEnumerable<EmployeeListResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}/departments", GetDepartmentsAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.ViewDetails))
            .Produces<List<DepartmentListResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetDepartmentsByEmployeeId");
        group.MapGet("/{id:guid}/employee/count", GetEmployeeCountAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.ViewUsers))
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithName("GetEmployeeCountByDepartmentId");
    }

    private async Task<IResult> AddEmployeeAsync(
        [FromRoute] Guid id,
        [FromBody] AddEmployeeToDepartmentCommand request,
        [FromServices] ICommandHandler<AddEmployeeToDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new AddEmployeeToDepartmentCommand(id, request.EmployeeId);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> RemoveEmployeeAsync(
        [FromRoute] Guid id,
        [FromBody] RemoveEmployeeFromDepartmentCommand request,
        [FromServices] ICommandHandler<RemoveEmployeeFromDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new RemoveEmployeeFromDepartmentCommand(id, request.EmployeeId);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> MoveEmployeeAsync(
        [FromRoute] Guid id,
        [FromBody] MoveEmployeeToDepartmentCommand request,
        [FromServices] ICommandHandler<MoveEmployeeToDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new MoveEmployeeToDepartmentCommand(id, request.EmployeeId);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }

    private async Task<IResult> GetEmployeesAsync(
        [FromRoute] Guid id,
        [FromQuery] int pagedNumber,
        [FromQuery] int pageSize,
        [FromServices] IQueryHandler<GetDepartmentEmployeesQuery, PagedResult<EmployeeListResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetDepartmentEmployeesQuery(id, pagedNumber, pageSize);
        var result = await handler.HandleAsync(query, ct);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetDepartmentsAsync(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetEmployeeDepartmentsQuery, List<DepartmentListResponse>> handler)
    {
        var query = new GetEmployeeDepartmentsQuery(id);
        var result = await handler.HandleAsync(query, CancellationToken.None);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblem();
    }
    private async Task<IResult> GetEmployeeCountAsync(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        return Results.Ok();
    }
}