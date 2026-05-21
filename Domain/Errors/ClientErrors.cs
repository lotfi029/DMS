namespace Domain.Errors;

public static class ClientErrors
{
    private const string _code = "clients";
    public static Error NotFound
        => Error.NotFound(
            $"{_code}.{nameof(NotFound)}",
            $"Client was not found.");

    public static Error AlreadyActive
        => Error.Conflict(
            $"{_code}.{nameof(AlreadyActive)}",
            $"Client is already active.");

    public static Error NotActive
        => Error.Conflict(
            $"{_code}.{nameof(NotActive)}",
            $"Client is not active.");

    public static Error CreatedFailed
        => Error.BadRequest(
            $"{_code}.{nameof(CreatedFailed)}",
            $"Failed to create client.");

    public static Error UpdateFailed
        => Error.BadRequest(
            $"{_code}.{nameof(UpdateFailed)}",
            $"Failed to update client.");
    public static Error DeleteFailed
       => Error.BadRequest(
           $"{_code}.{nameof(DeleteFailed)}",
           $"Failed to delete client.");

    public static Error ActivatedFailed
        => Error.BadRequest(
            $"{_code}.{nameof(ActivatedFailed)}",
            $"Failed to activate client.");

    public static Error DeactivatedFailed
        => Error.BadRequest(
            $"{_code}.{nameof(DeactivatedFailed)}",
            $"Failed to deactivate client.");
}