namespace Infrastructure.Persistence.Repositories;

public class DepartmentRepository(ApplicationDbContext dbContext) 
    : GenericRepository<Department>(dbContext), IDepartmentRepository
{
}



internal sealed class EmployeeRepository(ApplicationDbContext dbContext) 
    : GenericRepository<Employee>(dbContext), IEmployeeRepository
{
}