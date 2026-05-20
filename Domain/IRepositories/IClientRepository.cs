namespace Domain.IRepositories;

public interface IClientRepository : IGenericRepository<Client>
{
    Task<Client?> GetClientAsync(Expression<Func<Client, bool>> predicate, IEnumerable<string>? includeProperties = null, CancellationToken ct = default);
}
