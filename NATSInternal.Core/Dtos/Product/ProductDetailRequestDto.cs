namespace NATSInternal.Core.Dtos;

public class ProductDetailRequestDto : IRequestDto
{
    public int RecentSuppliesResultCount { get; set; } = 5;
    public int RecentOrdersResultCount { get; set; } = 5;
    public int RecentTreatmentsResultCount { get; set; } = 5;

    public void TransformValues()
    {
    }
}