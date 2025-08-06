namespace NATSInternal.Core.Dtos;

public class TreatmentItemResponseDto : IHasProductItemResponseDto
{
    public int Id { get; set; }
    public long ProductAmountPerUnit { get; set; }
    public long VatAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public ProductBasicResponseDto Product { get; set; }

    internal TreatmentItemResponseDto() { }

    internal TreatmentItemResponseDto(TreatmentItem treatmentItem)
    {
        Id = treatmentItem.Id;
        ProductAmountPerUnit = treatmentItem.ProductAmountPerUnit;
        VatAmountPerUnit = treatmentItem.VatAmount;
        Quantity = treatmentItem.Quantity;
        Product = new ProductBasicResponseDto(treatmentItem.Product);
    }
}
