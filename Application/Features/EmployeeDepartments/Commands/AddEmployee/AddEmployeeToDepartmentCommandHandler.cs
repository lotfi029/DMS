namespace Application.Features.EmployeeDepartments.Commands.AddEmployee;

public sealed record AddEmployeeToDepartmentCommand(
    Guid DepartmentId,
    Guid EmployeeId
    ) : ICommand;

internal sealed class AddEmployeeToDepartmentCommandValidator : AbstractValidator<AddEmployeeToDepartmentCommand>
{
    public AddEmployeeToDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department ID is required.");
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");
    }
}

internal sealed class AddEmployeeToDepartmentCommandHandler(
    IDepartmentDomainService departmentDomainService,
    IAuditService auditService,
    IUnitOfWork unitOfWork,
    ILogger<AddEmployeeToDepartmentCommandHandler> logger) : ICommandHandler<AddEmployeeToDepartmentCommand>
{
    public async Task<Result> HandleAsync(AddEmployeeToDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await departmentDomainService.AddEmployeeAsync(command.EmployeeId, command.DepartmentId, ct);

        if (result.IsFailure)
            return result.Error;
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation(LogMessages.Dept_UserAdded, command.EmployeeId, command.DepartmentId);

        await auditService.LogActionAsync(
            action: AuditAction.UserAddedToDepartment,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.DepartmentId.ToString(),
            description: $"User '{command.EmployeeId}' added to department '{command.DepartmentId}'.",
            ct: ct);

        return Result.Success();
    }
}