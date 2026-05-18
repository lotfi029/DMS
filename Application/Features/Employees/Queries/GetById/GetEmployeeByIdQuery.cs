using Application.DTOs.Employees;

namespace Application.Features.Employees.Queries.GetById;

public sealed record GetEmployeeByIdQuery(Guid Id) : IQuery<EmployeeResponse>;

internal sealed class GetEmployeeByIdQueryHandler(
    IEmployeeRepository employeeRepository) : IQueryHandler<GetEmployeeByIdQuery, EmployeeResponse>
{
    public async Task<Result<EmployeeResponse>> HandleAsync(GetEmployeeByIdQuery query, CancellationToken ct = default)
    {
        var employee = await employeeRepository.GetByIdAsync(x => x.Id == query.Id, [nameof(Employee.AppUser)], ct);
        if (employee is null)
            return EmployeeErrors.NotFound;

        var response = employee.Adapt<EmployeeResponse>();

        return response;
    }
}



//public sealed record EmployeeResponse(
//    Guid Id,
//    string AppUserId,
//    string FirstName,
//    string LastName,
//    string Email,
//    string UserName,
//    string JobTitle,
//    DateOnly HireDate,
//    bool IsActive,
//    DateTime CreatedAt,
//    DateTime LastLoginAt,
//    string? Notes,
//    Guid? DepartmentId,
//    string? DepartmentName
//    );



