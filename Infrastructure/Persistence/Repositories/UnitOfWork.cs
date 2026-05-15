using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default)
        => await dbContext.Database.BeginTransactionAsync(ct);

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        try
        {
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await RollBackTransactionAsync(transaction, ct);
            throw;
        }
    }

    public async Task RollBackTransactionAsync(IDbContextTransaction transaction, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        await transaction.RollbackAsync(ct);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await dbContext.SaveChangesAsync(ct);
    }

    public async ValueTask DisposeAsync()
    {
        await dbContext.DisposeAsync();
    }
}
