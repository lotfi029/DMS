using Application.DTOs.Employees;

namespace Application.Features.Employees.Queries.GetById;

public sealed record GetEmployeeByIdQuery(Guid Id) : IQuery<EmployeeResponse>;

internal sealed class GetEmployeeByIdQueryHandler(
    IEmployeeRepository employeeRepository,
    IEmployeeDepartmentRepository employeeDepartmentRepository,
    IRoleService roleService) : IQueryHandler<GetEmployeeByIdQuery, EmployeeResponse>
{
    public async Task<Result<EmployeeResponse>> HandleAsync(GetEmployeeByIdQuery query, CancellationToken ct = default)
    {
        var employee = await employeeRepository.GetByIdAsync(x => x.Id == query.Id, include: [nameof(Employee.AppUser)], ct: ct);
        if (employee is null)
            return EmployeeErrors.NotFound;

        var depts = await employeeDepartmentRepository.GetDepartmentAsync(query.Id, ct);
        var roles = await roleService.GetUserRolesAsync(employee.AppUserId, ct);

        var reponse = new EmployeeResponse(
            Id: employee.Id,
            UserId: employee.AppUserId,
            FirstName: employee.AppUser.FirstName,
            LastName: employee.AppUser.LastName,
            FullName: $"{employee.AppUser.FirstName} {employee.AppUser.LastName}",
            Email: employee.AppUser.Email!,
            UserName: employee.AppUser.UserName!,
            JobTitle: employee.JobTitle,
            ContractType: employee.ContractType.ToString()!,
            PhoneNumber: employee.PhoneNumber,
            EmergencyContactName: employee.EmergencyContactName,
            EmergencyContactPhone: employee.EmergencyContactPhone,
            HireDate: employee.HireDate,
            EndDate: employee.EndDate,
            IsActive: employee.IsActive,
            CreatedAt: employee.CreatedAt,
            LastLoginAt: employee.AppUser.LastLoginAt,
            Notes: employee.Notes,
            Departments: depts.Select(d => new DepartmentFromEmployeeResponse(d.DepartmentId, d.Department.Name)),
            Roles: roles.Value!.Select(r => new RoleForEmployeeResponse(r.Id, r.RoleName)));

        return reponse;
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



