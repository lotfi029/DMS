using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal sealed class UserPermissionOverrideConfiguration
    : IEntityTypeConfiguration<UserPermissionOverride>
{
    public void Configure(EntityTypeBuilder<UserPermissionOverride> builder)
    {
        builder.ToTable("user_permission_overrides", "dms");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.UserId)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.Permission)
            .HasMaxLength(200).IsRequired();
        builder.Property(x => x.GrantedById)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.Reason)
            .HasMaxLength(500);
        builder.Property(x => x.IsGranted).IsRequired();

        
        builder.HasIndex(x => new { x.UserId, x.Permission })
            .IsUnique()
            .HasDatabaseName("IX_user_permission_overrides_UserId_Permission");

        builder.HasIndex(x => x.ExpiresAt)
            .HasDatabaseName("IX_user_permission_overrides_ExpiresAt");

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}