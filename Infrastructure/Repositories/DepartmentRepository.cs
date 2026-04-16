namespace Infrastructure.Repositories;

public class DepartmentRepository(ApplicationDbContext dbContext) 
    : GenericRepository<Department>(dbContext), IDepartmentRepository
{
}
