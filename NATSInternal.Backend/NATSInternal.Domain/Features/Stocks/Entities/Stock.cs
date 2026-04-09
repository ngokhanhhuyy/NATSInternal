using NATSInternal.Domain.Exceptions;
using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Stocks;

internal class Stock : AbstractAggregateRootEntity
{
    #region Constructors
    #nullable disable
    private Stock() { }
    #nullable enable

    public Stock(int stockingQuantity, int resupplyThresholdQuantity, Guid productId)
    {
        PopulateProperties(stockingQuantity, resupplyThresholdQuantity);
        ProductId = productId;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int StockingQuantity { get; private set; }
    public int? ResupplyThresholdQuantity { get; private set; }
    #endregion
    
    #region ForeignKeyProperties
    public Guid ProductId { get; private set; }
    #endregion
    
    #region Methods
    public void IncrementStockingQuantity(int quantityToIncrement)
    {
        if (quantityToIncrement <= 0)
        {
            throw new DomainException("Quantity to increment must be greater than 0.");
        }
        
        StockingQuantity += quantityToIncrement;
    }
    
    public void DecrementStockingQuantity(int quantityToDecrement)
    {
        if (quantityToDecrement <= 0)
        {
            throw new DomainException("Quantity to decrement must be greater than 0.");
        }

        if (StockingQuantity - quantityToDecrement < 0)
        {
            throw new DomainException("Quantity to decrement must be greater or equal to stocking quantity");
        }
        
        StockingQuantity -= quantityToDecrement;
    }

    public void Update(int stockingQuantity, int? resupplyThresholdQuantity)
    {
        PopulateProperties(stockingQuantity, resupplyThresholdQuantity);
    }
    #endregion
    
    #region PrivateMethods
    private void PopulateProperties(int stockingQuantity, int? resupplyThresholdQuantity)
    {
        StockingQuantity = stockingQuantity;
        ResupplyThresholdQuantity = resupplyThresholdQuantity == 0 ? null : resupplyThresholdQuantity;
    }
    #endregion
}