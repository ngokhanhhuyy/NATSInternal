namespace NATSInternal.Core.DbContext;

internal class DailySummaryConfiguration : IEntityTypeConfiguration<DailySummary>
{
    #region Methods
    public void Configure(EntityTypeBuilder<DailySummary> entityBuilder)
    {
        entityBuilder.HasKey(ds => new { ds.RecordedYear, ds.RecordedMonth, ds.RecordedDay });
        entityBuilder
            .HasOne(ds => ds.MonthlySummary)
            .WithMany(ms => ms.DailySummaries)
            .HasForeignKey(ds => new { ds.RecordedYear, ds.RecordedMonth })
            .HasConstraintName("FK__daily_summaries__monthly_summaries__recorded_year__recorded_month")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
    #endregion
}
