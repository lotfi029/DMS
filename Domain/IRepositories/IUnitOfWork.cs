namespace Domain.IRepositories;

public interface IUnitOfWork : IAsyncDisposable
{   
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
