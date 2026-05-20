namespace Application.Features.Departments.Commands.Deactivate;

public sealed record DeactivateDepartmentCommand(Guid Id, Guid? NewDepartmentId) : ICommand;

internal sealed class DeactivateDepartmentCommandValidator : AbstractValidator<DeactivateDepartmentCommand>
{
    public DeactivateDepartmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Department ID is required.");
        RuleFor(x => x.NewDepartmentId).NotEmpty().When(x => x.NewDepartmentId != null).WithMessage("New department ID is required.");
    }
}

internal sealed class DeactivateDepartmentCommandHandler(
    IDepartmentDomainService departmentService,
    IAuditService auditService,
    ILogger<DeactivateDepartmentCommandHandler> logger
    ) : ICommandHandler<DeactivateDepartmentCommand>
{
    public async Task<Result> HandleAsync(DeactivateDepartmentCommand command, CancellationToken ct = default)
    {
        if (await departmentService.DeactivateAsync(command.Id, command.NewDepartmentId, ct) is { IsFailure: true } result)
        {
            logger.LogWarning(
                "Failed to deactivate department with ID {DepartmentId}. Error: {ErrorMessage}",
                command.Id, result.Error.ToString());
            return Result.Failure(result.Error);
        }

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentDeactivated,
            module: AuditModules.Departments,
            entityName: AuditModules.Departments,
            entityId: command.Id.ToString(),
            description: $"Department deactivated with ID {command.Id}",
            outcome: AuditOutcome.Success,
            ct: ct);

        return Result.Success();
    }
}