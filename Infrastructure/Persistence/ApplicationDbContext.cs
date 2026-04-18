using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>(options)
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dms");

        modelBuilder.Entity<ApplicationUser>().ToTable("users");
        modelBuilder.Entity<ApplicationRole>().ToTable("roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("user_roles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("user_claims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("user_tokens");
        modelBuilder.Entity<ApplicationRoleClaim>().ToTable("role_claims");
        modelBuilder.Entity<Department>().ToTable("departments");
        modelBuilder.Entity<AuditLog>().ToTable("audit_logs");
        modelBuilder.Entity<ApplicationRoleClaim>()
            .Property(rc => rc.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
