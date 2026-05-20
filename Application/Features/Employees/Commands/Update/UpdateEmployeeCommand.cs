using Application.DTOs.Employees;

namespace Application.Features.Employees.Commands.Update;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    UpdateEmployeeRequest Request
) : ICommand;
internal sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Request.FirstName)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Request.FirstName));
        
        RuleFor(x => x.Request.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.Request.LastName));

        RuleFor(x => x.Request.JobTitle)
            .NotEmpty()
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Request.JobTitle));

        RuleFor(x => x.Request.Notes)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.Request.Notes));

        RuleFor(x => x.Request.EndDate)
            .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .When(x => x.Request.EndDate.HasValue);

        RuleFor(x => x.Request.ContractType)
            .IsInEnum()
            .When(x => x.Request.ContractType.HasValue);

        RuleFor(x => x.Request.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .When(x => !string.IsNullOrEmpty(x.Request.PhoneNumber))
            .WithMessage("Phone number must be in E.164 format.");

        RuleFor(x => x.Request.EmergencyContactPhone)
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .When(x => !string.IsNullOrEmpty(x.Request.EmergencyContactPhone))
            .WithMessage("Emergency contact phone number must be in E.164 format.");

        RuleFor(x => x.Request.EmergencyContactName)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Request.EmergencyContactName));
    }
}

internal sealed class UpdateEmployeeCommandHandler(
    IEmployeeDomainService employeeDomainService,
    IAuditService auditService,
    ILogger<UpdateEmployeeCommandHandler> logger
    ) : ICommandHandler<UpdateEmployeeCommand>
{
    public async Task<Result> HandleAsync(UpdateEmployeeCommand command, CancellationToken ct = default)
    {
        var updateResult = await employeeDomainService.UpdateAsync(
            id: command.Id,
            firstName: command.Request.FirstName,
            lastName: command.Request.LastName,
            jobTitle: command.Request.JobTitle,
            contractType: command.Request.ContractType,
            phoneNumber: command.Request.PhoneNumber,
            emergencyContactName: command.Request.EmergencyContactName,
            emergencyContactPhone: command.Request.EmergencyContactPhone,
            notes: command.Request.Notes,
            ct: ct);

        if (updateResult.IsFailure)
        {
            logger.LogWarning("Failed to update employee with ID {EmployeeId}. Error: {ErrorMessage}", command.Id, updateResult.Error.ToString());
            
            await auditService.LogActionAsync(
                action: AuditAction.EmployeeUpdated,
                module: AuditModules.Employees,
                entityName: AuditEntityNames.Employee,
                entityId: command.Id.ToString(),
                outcome: AuditOutcome.Failure,
                description: $"Failed to update employee with ID {command.Id}.",
                ct: ct);

            return updateResult.Error;
        }

        await auditService.LogActionAsync(
            action: AuditAction.EmployeeUpdated,
            module: AuditModules.Employees,
            entityName: AuditEntityNames.Employee,
            entityId: command.Id.ToString(),
            outcome: AuditOutcome.Success,
            description: $"Employee '{command.Request.FirstName} {command.Request.LastName}' updated successfully.",
            ct: ct);

        return Result.Success();
    }
}