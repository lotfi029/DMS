using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class AppRoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
{
    public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
    {
        builder.Property(rc => rc.DisplayName)
            .HasMaxLength(256)
            .HasColumnName("display_name");

        builder.Property(rc => rc.Description)
            .HasMaxLength(1000)
            .HasColumnName("description");

        builder.Property(rc => rc.Group)
            .HasMaxLength(256)
            .HasColumnName("group");
    }
}