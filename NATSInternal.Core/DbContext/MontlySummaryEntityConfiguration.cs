namespace NATSInternal.Core.DbContext;

internal class MonthlySummaryEntityConfiguration : IEntityTypeConfiguration<MonthlySummary>
{
    #region Methods
    public void Configure(EntityTypeBuilder<MonthlySummary> entityBuilder)
    {
        entityBuilder.HasKey(ms => new { ms.RecordedYear, ms.RecordedMonth });
    }
    #endregion
}
