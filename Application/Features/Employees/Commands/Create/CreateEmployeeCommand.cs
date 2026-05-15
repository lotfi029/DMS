namespace Application.Features.Employees.Commands.Create;

public sealed record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string JobTitle,
    string? RoleId,
    Guid? DepartmentId,
    string? Notes) : ICommand<Guid>;

internal sealed class CreateEmployeeCommandHandler(
    IEmployeeDomainService employeeService,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IAuditService auditService,
    IDepartmentRepository departmentRepository,
    ILogger<CreateEmployeeCommandHandler> logger) : ICommandHandler<CreateEmployeeCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CreateEmployeeCommand command, CancellationToken ct = default)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            if (!await departmentRepository.ExistsAsync(x => x.Id == command.DepartmentId, ct))
                return DepartmentErrors.NotFound;

            var registerRequest = new RegisterRequest(
                command.FirstName,
                command.LastName,
                command.Password,
                command.Email,
                command.UserName);

            var registerResult = await authService.RegisterAsync(command.RoleId!, UserType.Employee, registerRequest, ct);
            if (registerResult.IsFailure)
                return registerResult.Error;

            var employeeResult = employeeService.Create(
                userId: registerResult.Value!,
                departmentId: command.DepartmentId!.Value,
                jobTitle: command.JobTitle,
                notes: command.Notes
                );

            if (employeeResult.IsFailure)
                return employeeResult.Error;

            await unitOfWork.SaveChangesAsync(ct);

            await auditService.LogActionAsync(
                AuditAction.EmployeeCreated,
                module: AuditModules.Employees,
                entityName: AuditEntityNames.Employee,
                entityId: employeeResult.ToString(),
                outcome: AuditOutcome.Success,
                ct: ct);

            await transaction.CommitAsync(ct);
            return employeeResult;
        }
        catch 
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}
// first register user (role, user metadata).
// second create employee with the registered user id and other employee-specific data (department, job title, notes).