namespace Application.Features.Departments.Commands.Update;

public sealed record UpdateDepartmentCommand(
    Guid Id,
    UpdateDepartmentRequest Request) : ICommand;

internal sealed class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Department ID is required.");
        RuleFor(x => x.Request.Name)
            .NotNull().WithMessage("Department name is required.")
            .Length(3, 100).WithMessage("Department name must be between 3 and 100 characters.")
            .When(x => x.Request.Name != null);

        RuleFor(x => x.Request.Description)
            .MaximumLength(500).WithMessage("Department description must not exceed 500 characters.")
            .When(x => x.Request.Description != null);
    }
}

internal sealed class UpdateDepartmentCommandHandler(
    IDepartmentDomainService domainService,
    IAuditService auditService,
    ILogger<UpdateDepartmentCommandHandler> logger) : ICommandHandler<UpdateDepartmentCommand>
{
    public async Task<Result> HandleAsync(UpdateDepartmentCommand command, CancellationToken ct = default)
    {
        var result = await domainService.UpdateAsync(command.Id, command.Request.Name!, command.Request.Description, ct);

        if (result.IsFailure)
        {
            logger.LogWarning(LogMessages.Dept_NotFound, command.Id);
            return result.Error;
        }

        logger.LogInformation(LogMessages.Dept_Updated, command.Id);

        await auditService.LogActionAsync(
            action: AuditAction.DepartmentUpdated,
            module: AuditModules.Departments,
            entityName: AuditEntityNames.Department,
            entityId: command.Id.ToString(),
            description: $"Department '{command.Id}' updated.",
            ct: ct);

        return Result.Success();
    }
}

