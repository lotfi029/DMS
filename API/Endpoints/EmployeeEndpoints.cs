using Application.DTOs.Employees;

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

        group.MapPost("create", CreateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Create))
            .Produces<Guid>(StatusCodes.Status201Created);

        group.MapPut("update", UpdateAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Update))
            .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("delete", DeleteAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Delete))
            .Produces(StatusCodes.Status204NoContent);

        group.MapGet("{id:guid}", GetByIdAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.ViewDetails))
            .Produces<EmployeeResponse>(StatusCodes.Status200OK);

        group.MapGet("", GetAllAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Read))
            .Produces<IEnumerable<EmployeeResponse>>(StatusCodes.Status200OK);

        group.MapGet("by-role/{roleName}", GetByRoleAsync)
            .WithMetadata(new HasPermissionAttribute(DefaultPermissions.Employees.Read))
            .Produces<IEnumerable<EmployeeResponse>>(StatusCodes.Status200OK);
    }

    private Task<IResult> CreateAsync()
    {
        throw new NotImplementedException();
    }
    private Task<IResult> UpdateAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    private Task<IResult> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    private Task<IResult> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    private Task<IResult> GetAllAsync()
    {
        throw new NotImplementedException();
    }
    private Task<IResult> GetByRoleAsync(string roleName)
    {
        throw new NotImplementedException();
    }
}