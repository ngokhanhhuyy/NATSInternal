namespace NATSInternal.Core.Dtos;

public class ProductDetailRequestDto : IRequestDto
{
    #region Properties
    public int? RecentSuppliesResultCount { get; set; }
    public int? RecentOrdersResultCount { get; set; }
    public int? RecentTreatmentsResultCount { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}