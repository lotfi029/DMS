namespace Application.Features.Employees.Commands.Deactivate;

public sealed record DeactivateEmployeeCommand(Guid Id) : ICommand;
internal sealed class DeactivateEmployeeCommandValidator : AbstractValidator<DeactivateEmployeeCommand>
{
    public DeactivateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Employee ID is required.");
    }
}
internal sealed class DeactivateEmployeeCommandHandler(
    IEmployeeDomainService employeeService,
    IAuditService auditService,
    ILogger<DeactivateEmployeeCommandHandler> logger
    ) : ICommandHandler<DeactivateEmployeeCommand>
{
    public async Task<Result> HandleAsync(DeactivateEmployeeCommand command, CancellationToken ct = default)
    {
        if (await employeeService.DeactivateAsync(command.Id, ct) is { IsFailure: true } result)
        {
            logger.LogWarning("Deactivate employee failed for {EmployeeId}: {Error}",
                command.Id, result.Error.Description);

            return result.Error;
        }
        logger.LogInformation("Employee with id {EmployeeId} deactivated", command.Id);

        await auditService.LogActionAsync(
            action: AuditAction.EmployeeDeactivated,
            module: AuditModules.Employees,
            entityName: AuditEntityNames.Employee,
            entityId: command.Id.ToString(),
            description: $"Employee '{command.Id}' deactivated.",
            ct: ct);

        return Result.Success();
    }
}