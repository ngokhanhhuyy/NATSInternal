namespace NATSInternal.Core.DbContext;

internal class DailySummaryConfiguration : IEntityTypeConfiguration<DailySummary>
{
    #region Methods
    public void Configure(EntityTypeBuilder<DailySummary> entityBuilder)
    {
        entityBuilder.HasKey(ds => ds.Id);
        entityBuilder
            .HasOne(dfs => dfs.Monthly)
            .WithMany(mfs => mfs.DailyStats)
            .HasForeignKey(dfs => dfs.MonthlyId)
            .HasConstraintName("FK__daily_summaries__monthly_summaries__monthly_summary_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasIndex(dfs => dfs.RecordedDate)
            .IsUnique()
            .HasDatabaseName("IX__daily_summaries__recorded_date");
    }
    #endregion
}
