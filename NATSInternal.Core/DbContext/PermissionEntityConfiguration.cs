namespace NATSInternal.Core.DbContext;

internal class PermissionEntityConfiguration : IEntityTypeConfiguration<Permission>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Permission> entityBuilder)
    {
        entityBuilder.HasKey(p => p.Id);
        entityBuilder
            .HasOne(p => p.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(p => p.RoleId)
            .HasConstraintName("FK__permissions__roles__role_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
    }
    #endregion
}
