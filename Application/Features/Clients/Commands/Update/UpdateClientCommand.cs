using Application.DTOs.Clients;

namespace Application.Features.Clients.Commands.Update;

public sealed record UpdateClientCommand(
    Guid Id, UpdateClientRequest Request) : ICommand;

internal sealed class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Client ID is required.");
        RuleFor(x => x.Request.AppUserId)
            .NotEmpty().WithMessage("App User ID is required.");
        RuleFor(x => x.Request.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .When(x => x.Request.LastName is not null);
        
        RuleFor(x => x.Request.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .When(x => x.Request.LastName is not null);
        RuleFor(x => x.Request.Address)
            .NotEmpty().WithMessage("Address is required.")
            .When(x => x.Request.Address is not null);
        RuleFor(x => x.Request.Notes)
            .NotEmpty().WithMessage("Notes is required.")
            .When(x => x.Request.Notes is not null);
    }
}

internal sealed class UpdateClientCommandHandler(
    IClientDomainService clientService,
    IAuditService auditService,
    ILogger<UpdateClientCommandHandler> logger) : ICommandHandler<UpdateClientCommand>
{

    public async Task<Result> HandleAsync(UpdateClientCommand request, CancellationToken ct)
    {
        var updatedResult = await clientService.UpdateAsync(
            request.Id, 
            request.Request.AppUserId,
            firstName: request.Request.FirstName,
            lastName: request.Request.LastName,
            address: request.Request.Address,
            notes: request.Request.Notes,
            ct);

        if (updatedResult.IsFailure)
        {
            logger.LogError(LogMessages.Client_UpdateFailed, request.Id, updatedResult.Error.ToString());
            return updatedResult.Error;
        }    

        logger.LogInformation(LogMessages.Client_Updated, request.Id);
        await auditService.LogActionAsync(
            action: AuditAction.ClientUpdated,
            module: AuditModules.Clients,
            entityName: AuditEntityNames.Client,
            entityId: request.Id.ToString(),
            description: $"Client with ID {request.Id} was updated.",
            outcome: AuditOutcome.Success,
            ct: ct);

        return updatedResult;
    }
}