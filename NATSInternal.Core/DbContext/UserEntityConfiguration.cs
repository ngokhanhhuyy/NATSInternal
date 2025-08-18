namespace NATSInternal.Core.DbContext;

internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    #region Methods
    public void Configure(EntityTypeBuilder<User> entityBuilder)
    {
        entityBuilder.HasKey(u => u.Id);
        entityBuilder
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>(
                userRole => userRole
                    .HasOne(ur => ur.Role)
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .HasConstraintName("FK__user_roles__roles__role_id")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired(),
                userRole => userRole
                    .HasOne(ur => ur.User)
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId)
                    .HasConstraintName("FK__user_roles__users__user_id")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired());
        entityBuilder
            .HasIndex(u => u.UserName)
            .IsUnique()
            .HasDatabaseName("IX__user_roles__user_name");
        entityBuilder.Property(u => u.RowVersion).IsRowVersion();
        entityBuilder.HasQueryFilter(u => !u.IsDeleted);
    }
    #endregion
}