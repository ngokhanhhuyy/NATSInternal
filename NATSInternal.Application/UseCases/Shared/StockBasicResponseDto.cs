using NATSInternal.Domain.Features.Stocks;

namespace NATSInternal.Application.UseCases.Shared;

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
    public Guid Id { get; }
    public int StockingQuantity { get; }
    public int? ResupplyThresholdQuantity { get; }
    #endregion
}