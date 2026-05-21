using Application.Abstractions.Pagination;
using Application.DTOs.Employees;
using Application.Features.EmployeeDepartments.Commands.AddEmployee;
using Application.Features.EmployeeDepartments.Commands.MoveUser;
using Application.Features.EmployeeDepartments.Commands.RemoveUser;
using Application.Features.EmployeeDepartments.Queries.GetEmployees;

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
    private async Task<IResult> RemoveUserAsync(
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
    private async Task<IResult> MoveUserAsync(
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
}