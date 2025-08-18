namespace NATSInternal.Core.DbContext;

internal class MonthlySummaryEntityConfiguration : IEntityTypeConfiguration<MonthlySummary>
{
    #region Methods
    public void Configure(EntityTypeBuilder<MonthlySummary> entityBuilder)
    {
        entityBuilder.HasKey(ms => ms.Id);
        entityBuilder
            .HasIndex(dfs => new { dfs.RecordedMonth, dfs.RecordedYear })
            .HasDatabaseName("IX__monthly_summaries__recorded_month__recorded_year")
            .IsUnique();
    }
    #endregion
}
