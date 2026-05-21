namespace Application.Features.Clients.Commands.Activate;

public sealed record ActivateClientCommand(
    Guid ClientId
) : ICommand;

internal sealed class ActivateClientCommandValidator : AbstractValidator<ActivateClientCommand>
{
    public ActivateClientCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("Client ID is required.");
    }
}

internal sealed class ActivateClientCommandHandler(
    IClientDomainService clientService,
    IAuditService auditService,
    ILogger<ActivateClientCommandHandler> logger) : ICommandHandler<ActivateClientCommand>
{
    public async Task<Result> HandleAsync(ActivateClientCommand request, CancellationToken ct)
    {
        var activateResult = await clientService.ActiveAsync(request.ClientId, ct: ct);
        if (activateResult.IsFailure)
        {
            logger.LogError(LogMessages.Client_ActivatedFailed, request.ClientId, activateResult.Error.ToString());
            return activateResult.Error;
        }

        logger.LogInformation(LogMessages.Client_Activated, request.ClientId);

        await auditService.LogActionAsync(
            action: AuditAction.ClientActivated,
            module: AuditModules.Clients,
            entityName: AuditEntityNames.Client,
            entityId: request.ClientId.ToString(),
            description: $"Client with ID {request.ClientId} was activated.",
            outcome: AuditOutcome.Success,
            ct: ct);
        return Result.Success();
    }
}

