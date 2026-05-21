using Application.Abstractions.Data;
using Application.DTOs.Employees;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Employees.Queries.GetById;

public sealed record GetEmployeeByIdQuery(Guid Id) : IQuery<EmployeeResponse>;

internal sealed class GetEmployeeByIdQueryHandler(
    IApplicationDbContext context) : IQueryHandler<GetEmployeeByIdQuery, EmployeeResponse>
{
    public async Task<Result<EmployeeResponse>> HandleAsync(GetEmployeeByIdQuery query, CancellationToken ct = default)
    {
        var employee = await context.EmployeeProfiles
            .AsNoTracking()
            .Where(e => e.EmployeeId == query.Id)
            .ToListAsync(ct);

        var response = employee
            .GroupBy(e => new
            {
                e.EmployeeId,
                e.UserId,
                e.FirstName,
                e.LastName,
                e.Email,
                e.UserName,
                e.JobTitle,
                e.UserPhone,
                e.EmergencyContactName,
                e.EmergencyContactPhone,
                e.HireDate,
                e.EndDate,
                e.EmployeeIsActive,
                e.CreatedAt,
                e.LastLoginAt,
                e.Notes
            }).Select(e => new EmployeeResponse
            (
                e.Key.EmployeeId,
                e.Key.UserId,
                e.Key.FirstName,
                e.Key.LastName,
                $"{e.First().FirstName} {e.First().LastName}",
                e.Key.Email,
                e.Key.UserName!,
                e.Key.JobTitle,
                "{e.First().ContractType}",
                e.Key.UserPhone,
                e.Key.EmergencyContactName,
                e.Key.EmergencyContactPhone,
                e.Key.HireDate,
                e.Key.EndDate,
                e.Key.EmployeeIsActive,
                e.Key.CreatedAt,
                e.Key.LastLoginAt,
                e.Key.Notes,
                e.SelectMany(x => x.DepartmentId.HasValue ? new[] { new DepartmentFromEmployeeResponse(x.DepartmentId!.Value, x.DepartmentName!) } : default!),
                e.SelectMany(x => !string.IsNullOrWhiteSpace(x.UserRoleId!) ? new[] { new RoleForEmployeeResponse(x.UserRoleId, x.RoleName!) } : default!)
            ))
            .FirstOrDefault();

        if (response is null)
            return EmployeeErrors.NotFound;

        return response;
    }
}