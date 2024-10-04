namespace NATSInternal.Services.Dtos;

public class TreatmentItemResponseDto : IProductEngageableItemResponseDto
{
    public int Id { get; set; }
    public long AmountBeforeVatPerUnit { get; set; }
    public decimal VatAmount { get; set; }
    public int Quantity { get; set; }
    public ProductBasicResponseDto Product { get; set; }

    internal TreatmentItemResponseDto() { }

    internal TreatmentItemResponseDto(TreatmentItem treatmentItem)
    {
        Id = treatmentItem.Id;
        AmountBeforeVatPerUnit = treatmentItem.AmountBeforeVatPerUnit;
        VatAmount = treatmentItem.VatAmount;
        Quantity = treatmentItem.Quantity;
        Product = new ProductBasicResponseDto(treatmentItem.Product);
    }
}
