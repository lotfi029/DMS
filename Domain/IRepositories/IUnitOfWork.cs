using Microsoft.EntityFrameworkCore.Storage;

namespace Domain.IRepositories;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
