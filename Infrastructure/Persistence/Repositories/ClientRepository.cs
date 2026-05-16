namespace Infrastructure.Persistence.Repositories;

internal sealed class ClientRepository(ApplicationDbContext dbContext)
    : GenericRepository<Client>(dbContext), IClientRepository
{
}