namespace Infrastructure.Persistence.Repositories;

internal sealed class ClientRepository(ApplicationDbContext dbContext)
    : GenericRepository<Client>(dbContext), IClientRepository
{
    public async Task<Client?> GetClientAsync(
        Expression<Func<Client, bool>> predicate,
        IEnumerable<string>? includeProperties = null,
        CancellationToken ct = default)
    {
        IQueryable<Client> query = DbContext.Clients;
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.FirstOrDefaultAsync(predicate, ct);
    }
}