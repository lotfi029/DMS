using Application.DTOs.Departments;
using Application.DTOs.Employees;
using Application.DTOs.Users;
using Application.Features.Departments.Commands.AddUser;
using Application.Features.Departments.Commands.AssignHead;
using Application.Features.Departments.Commands.Create;
using Application.Features.Departments.Commands.Delete;
using Application.Features.Departments.Commands.MoveUser;
using Application.Features.Departments.Commands.RemoveUser;
using Application.Features.Departments.Commands.Update;
using Application.Features.Departments.Queries.Get;
using Application.Features.Departments.Queries.Get.GetUsers;
using Application.Features.Departments.Queries.GetAll;

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

        group.MapPost("/{id:guid}/add-user", AddUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.AssignToUser))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/remove-user", RemoveUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.RemoveFromUser))
            .Produces(StatusCodes.Status204NoContent);
        group.MapPut("/{id:guid}/move-user", MoveUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.MoveUser))
            .Produces(StatusCodes.Status204NoContent);

        group.MapPut("/{id:guid}/assign-head", AssignHeadAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.AssignDepartmentHead))
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
        
        group.MapGet("/{id:guid}/users", GetUserAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Departments.ViewUsers))
            .Produces<IEnumerable<UserListResponse>>(StatusCodes.Status200OK);
    }
    
    private async Task<IResult> CreateAsync(
        [FromBody] CreateDepartmentRequest request,
        [FromServices] IValidator<CreateDepartmentRequest> validator,
        [FromServices] ICommandHandler<CreateDepartmentCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var command = new CreateDepartmentCommand(request);
        var result = await handler.HandleAsync(command, ct);
        return result.IsSuccess 
            ? Results.Created() 
            : result.ToProblem();
    }

    private async Task<IResult> AddUserAsync(
        [FromRoute] Guid id,
        [FromBody] DepartmentUserRequest request,
        [FromServices] IValidator<DepartmentUserRequest> validator,
        [FromServices] ICommandHandler<AddUserToDepartmentCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var command = new AddUserToDepartmentCommand(request.UserId, id);
        var result = await handler.HandleAsync(command, ct);
        
        return result.IsSuccess 
            ? Results.NoContent() 
            : result.ToProblem();
    }
    private async Task<IResult> RemoveUserAsync(
        [FromRoute] Guid id,
        [FromBody] DepartmentUserRequest request,
        [FromServices] IValidator<DepartmentUserRequest> validator,
        [FromServices] ICommandHandler<RemoveEmployeeFromDepartmentCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var command = new RemoveEmployeeFromDepartmentCommand(request.UserId, id);
        var result = await handler.HandleAsync(command, ct);
        
        return result.IsSuccess 
            ? Results.NoContent() 
            : result.ToProblem();
    }
    private async Task<IResult> MoveUserAsync(
        [FromRoute] Guid id,
        [FromBody] DepartmentUserRequest request,
        [FromServices] IValidator<DepartmentUserRequest> validator,
        [FromServices] ICommandHandler<MoveEmployeeToDepartmentCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());
        
        var command = new MoveEmployeeToDepartmentCommand(request.UserId, id);
        var result = await handler.HandleAsync(command, ct);
        
        return result.IsSuccess 
            ? Results.NoContent() 
            : result.ToProblem();
    }
    private async Task<IResult> AssignHeadAsync(
        [FromRoute] Guid id,
        [FromBody] DepartmentUserRequest request,
        [FromServices] ICommandHandler<AssignDepartmentHeadCommand> handler,
        CancellationToken ct)
    {
        var command = new AssignDepartmentHeadCommand(request.UserId, id);
        var result = await handler.HandleAsync(command, ct);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblem();
    }
    private async Task<IResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateDepartmentRequest request,
        [FromServices] IValidator<UpdateDepartmentRequest> validator,
        [FromServices] ICommandHandler<UpdateDepartmentCommand> handler,
        CancellationToken ct)
    {
        if (await validator.ValidateAsync(request, ct) is { IsValid: false } validationResult)
            return Results.ValidationProblem(validationResult.ToDictionary());

        var command = new UpdateDepartmentCommand(id, request);
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
    private async Task<IResult> GetUserAsync(
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
