namespace NATSInternal.Services.Dtos;

public class TreatmentItemUpdateHistoryDataDto : IProductExportableItemUpdateHistoryDataDto
{
    public int Id { get; set; }
    public long ProductAmountPerUnit { get; set; }
    public long VatAmountPerUnit { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }

    internal TreatmentItemUpdateHistoryDataDto(TreatmentItem item)
    {
        Id = item.Id;
        ProductAmountPerUnit = item.ProductAmountPerUnit;
        VatAmountPerUnit = item.VatAmountPerUnit;
        Quantity = item.Quantity;
        ProductName = item.Product.Name;
    }
}