namespace Application.Abstractions.Behaviors;

internal sealed class LoggingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken ct = default)
        {
            string requestName = typeof(TCommand).Name;
            
            logger.LogInformation("Handling command {RequestName}", requestName);

            Result<TResponse> result = await innerHandler.HandleAsync(command, ct);
            if (result.IsSuccess)
            {
                logger.LogInformation("Command {RequestName} handled successfully", requestName);
            }
            else
            {
                logger.LogWarning("Command {RequestName} failed with errors", requestName);
            }
            return result;
        }
    }
    internal sealed class CommandHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandHandler<TCommand>> logger)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> HandleAsync(TCommand command, CancellationToken ct = default)
        {
            string requestName = typeof(TCommand).Name;
            
            logger.LogInformation("Handling command {RequestName}", requestName);

            Result result = await innerHandler.HandleAsync(command, ct);
            if (result.IsSuccess)
            {
                logger.LogInformation("Command {RequestName} handled successfully", requestName);
            }
            else
            {
                logger.LogWarning("Command {RequestName} failed with errors", requestName);
            }
            return result;
        }
    }
    internal sealed class QueryHandler<TQuery, TResponse>(
        IQueryHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TQuery command, CancellationToken ct = default)
        {
            string requestName = typeof(TQuery).Name;

            logger.LogInformation("Handling query {RequestName}", requestName);

            Result<TResponse> result = await innerHandler.HandleAsync(command, ct);
            if (result.IsSuccess)
            {
                logger.LogInformation("Query {RequestName} handled successfully", requestName);
            }
            else
            {
                logger.LogWarning("Query {RequestName} failed with errors", requestName);
            }
            return result;
        }
    }

}
