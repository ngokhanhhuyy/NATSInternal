namespace NATSInternal.Core.DbContext;

internal class AnnouncementEntityConfiguration : IEntityTypeConfiguration<Announcement>
{
    #region Methods
    public void Configure(EntityTypeBuilder<Announcement> entityBuilder)
    {
        entityBuilder.HasKey(a => a.Id);
        entityBuilder
            .HasOne(a => a.CreatedUser)
            .WithMany(u => u.CreatedAnnouncements)
            .HasForeignKey(a => a.CreatedUserId)
            .HasConstraintName("FK__announcements__users__user_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
    }
    #endregion
}