namespace Application.Features.Clients.Commands.Deactivate;

public sealed record DeactivateClientCommand(
    Guid ClientId,
    string UserId
) : ICommand;

internal sealed class DeactivateClientCommandValidator : AbstractValidator<DeactivateClientCommand>
{
    public DeactivateClientCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("Client ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}

internal sealed class DeactivateClientCommandHandler(
    IClientDomainService clientService,
    IAuditService auditService,
    ILogger<DeactivateClientCommandHandler> logger) : ICommandHandler<DeactivateClientCommand>
{
    public async Task<Result> HandleAsync(DeactivateClientCommand request, CancellationToken ct)
    {
        var deactivateResult = await clientService.DeactivaAsync(request.ClientId, request.UserId, ct: ct);
        if (deactivateResult.IsFailure)
        {
            logger.LogError(LogMessages.Client_DeactivatedFailed, request.ClientId, deactivateResult.Error.ToString());
            return deactivateResult.Error;
        }
        logger.LogInformation(LogMessages.Client_Deactivated, request.ClientId);
        await auditService.LogActionAsync(
            action: AuditAction.ClientDeactivated,
            module: AuditModules.Clients,
            entityName: AuditEntityNames.Client,
            entityId: request.ClientId.ToString(),
            description: $"Client with ID {request.ClientId} was deactivated.",
            outcome: AuditOutcome.Success,
            ct: ct);
        return Result.Success();
    }
}