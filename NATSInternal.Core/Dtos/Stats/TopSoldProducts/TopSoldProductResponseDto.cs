namespace NATSInternal.Core.Dtos;

public class TopSoldProductResponseDto : IBasicResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public string ThumbnailUrl { get; set; }
    public long Amount { get; set; }
    public int Quantity { get; set; }

    internal TopSoldProductResponseDto(Product product, long amount, int quantity)
    {
        Id = product.Id;
        Name = product.Name;
        Unit = product.Unit;
        ThumbnailUrl = product.ThumbnailUrl;
        Amount = amount;
        Quantity = quantity;
    }
}
