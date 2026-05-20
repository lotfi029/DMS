namespace Application.Features.Clients.Commands.Create;

public sealed record CreateClientCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password,
    string Phone,
    string Address,
    string? Notes = null
) : ICommand<Guid>;

internal sealed class CreateClientCommandHandler(
    IUnitOfWork unitOfWork,
    IAuditService auditService,
    IAuthService authService,
    IClientRepository clientRepository
    ) : ICommandHandler<CreateClientCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CreateClientCommand command, CancellationToken ct = default)
    {
        var transaction = await unitOfWork.BeginTransactionAsync(ct);
        try
        {
            var registerResult = await authService.RegisterAsync(
                roleId: DefaultRoles.Client.Id,
                userType: UserType.Client,
                new RegisterRequest(
                    FirstName: command.FirstName,
                    LastName: command.LastName,
                    Password: command.Password,
                    Email: command.Email,
                    UserName: command.UserName,
                    PhoneNumber:  command.Phone
                    ),
                ct
                );

            if (registerResult.IsFailure)
            {
                await transaction.RollbackAsync(ct);
                return registerResult.Error;
            }

            var client = Client.Create(
                userId: registerResult.Value!,
                address: command.Address,
                notes: command.Notes
                );

            clientRepository.Add(client);
            await unitOfWork.SaveChangesAsync(ct);

            await auditService.LogActionAsync(
                action: AuditAction.ClientCreated,
                module: AuditModules.Clients,
                entityName: AuditEntityNames.Client,
                entityId: client.Id.ToString(),
                description: $"Client '{client.AppUserId}' created with email '{command.Email}'",
                outcome: AuditOutcome.Success,
                ct: ct
                );
            await transaction.CommitAsync(ct);
            return Result.Success(client.Id);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;

        }
    }
}