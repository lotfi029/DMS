namespace Application.Features.Clients.Commands.Delete;

public sealed record DeleteClientCommand(Guid Id, string UserId) : ICommand;

internal sealed class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Client ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}

internal sealed class DeleteClientCommandHandler(
    IClientDomainService clientService,
    IAuditService auditService,
    ILogger<DeleteClientCommandHandler> logger) : ICommandHandler<DeleteClientCommand>
{
    public async Task<Result> HandleAsync(DeleteClientCommand request, CancellationToken ct)
    {
        var deletedResult = await clientService.DeleteAsync(request.Id, request.UserId, ct: ct);
        if (deletedResult.IsFailure)
        {
            logger.LogError(LogMessages.Client_DeleteFailed, request.Id, deletedResult.Error.ToString());
            return deletedResult.Error;
        }

        logger.LogInformation(LogMessages.Client_Deleted, request.Id);

        await auditService.LogActionAsync(
            action: AuditAction.ClientDeleted,
            module: AuditModules.Clients,
            entityName: AuditEntityNames.Client,
            entityId: request.Id.ToString(),
            description: $"Client with ID {request.Id} was deleted.",
            outcome: AuditOutcome.Success,
            ct: ct);

        return Result.Success();
    }
}


