namespace NATSInternal.Core.Features.Products;

public class StockBasicResponseDto
{
    #region Constructors
    internal StockBasicResponseDto(Stock stock)
    {
        Id = stock.Id;
        StockingQuantity = stock.StockingQuantity;
        ResupplyThresholdQuantity = stock.ResupplyThresholdQuantity;
    }
    #endregion

    #region Properties
    public int Id { get; }
    public int StockingQuantity { get; }
    public int? ResupplyThresholdQuantity { get; }
    #endregion
}