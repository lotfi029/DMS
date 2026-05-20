using Application.DTOs.Departments;
using Application.DTOs.Employees;
using Application.Features.Departments.Queries.GetEmployees;
using Application.Features.EmployeeDepartments.Commands;

namespace API.Endpoints;

internal sealed class DepartmentEmployeeEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/department-employees")
            .WithTags("Department Employees")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("/{id:guid}/add-employee", AddUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.AssignToUser))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/remove-employee", RemoveUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.RemoveFromUser))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/move-employee", MoveUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.MoveUser))
            .Produces(StatusCodes.Status204NoContent);

        group.MapGet("/{id:guid}/employees", GetEmployeesAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.ViewUsers))
            .Produces<IEnumerable<EmployeeListResponse>>(StatusCodes.Status200OK);

    }

    private async Task<IResult> AddUserAsync(
        [FromRoute] Guid id,
        [FromBody] DepartmentEmployeeRequest request,
        [FromServices] ICommandHandler<EmployeeDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new EmployeeDepartmentCommand(request.EmployeeId, id);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> RemoveUserAsync(
        [FromRoute] Guid id,
        [FromBody] DepartmentEmployeeRequest request,
        [FromServices] ICommandHandler<EmployeeDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new EmployeeDepartmentCommand(request.EmployeeId, id);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> MoveUserAsync(
        [FromRoute] Guid id,
        [FromBody] DepartmentEmployeeRequest request,
        [FromServices] ICommandHandler<EmployeeDepartmentCommand> handler,
        CancellationToken ct)
    {
        var command = new EmployeeDepartmentCommand(request.EmployeeId, id);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }

    private async Task<IResult> GetEmployeesAsync(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetDepartmentEmployeesQuery, List<EmployeeListResponse>> handler,
        CancellationToken ct)
    {
        var query = new GetDepartmentEmployeesQuery(id);
        var result = await handler.HandleAsync(query, ct);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblem();
    }
}