namespace Application.Features.Employees.Commands.Active;

public sealed record ActivateEmployeeCommand(Guid Id) : ICommand;

internal sealed class ActivateEmployeeCommandValidator : AbstractValidator<ActivateEmployeeCommand>
{
    public ActivateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Employee ID is required.");
    }
}

internal sealed class ActivateEmployeeCommandHandler(
    IEmployeeDomainService employeeService,
    IAuditService auditService,
    ILogger<ActivateEmployeeCommandHandler> logger
    ) : ICommandHandler<ActivateEmployeeCommand>
{
    public async Task<Result> HandleAsync(ActivateEmployeeCommand command, CancellationToken ct = default)
    {
        if (await employeeService.ActivateAsync(command.Id, ct) is { IsFailure: true } result)
        {
            logger.LogWarning("Activate employee failed for {EmployeeId}: {Error}",
                command.Id, result.Error.Description);
            return result.Error;
        }

        logger.LogInformation("Employee with ID {EmployeeId} activated.", command.Id);

        await auditService.LogActionAsync(
            action: AuditAction.EmployeeActivated,
            module: AuditModules.Employees,
            entityName: AuditEntityNames.Employee,
            entityId: command.Id.ToString(),
            outcome: AuditOutcome.Success,
            ct: ct
        );

        return Result.Success();
    }
}