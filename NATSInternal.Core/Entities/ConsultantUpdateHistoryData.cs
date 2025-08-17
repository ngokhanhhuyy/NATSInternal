namespace NATSInternal.Core.Entities;

internal class ConsultantUpdateHistoryData
{
    #region Properties
    public long AmountBeforeVat { get; set; }
    public long VatAmount { get; set; }
    public string? Note { get; set; }
    public DateTime StatsDateTime { get; set; }
    #endregion
}
