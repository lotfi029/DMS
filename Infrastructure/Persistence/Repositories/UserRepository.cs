namespace Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext dbContext) 
    : GenericRepository<ApplicationUser>(dbContext), IUserRepository
{
}
