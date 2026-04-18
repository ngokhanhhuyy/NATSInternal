using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Supplies;

internal class SupplyItem : AbstractEntity
{
    #region Constructors
    private SupplyItem() { }

    public SupplyItem(long amountPerUnit, int quantity)
    {
        AmountPerUnit = amountPerUnit;
        Quantity = quantity;
    }
    #endregion

    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public long AmountPerUnit { get; private set; }
    public int Quantity { get; private set; }
    #endregion

    #region ForeignKeyProperties
    public Guid SupplyId { get; private set; }
    public Guid ProductId { get; private set; }
    #endregion

    #region ComputedProperties
    public long Amount => AmountPerUnit * Quantity;
    #endregion

    #region Methods
    public bool Update(long amountPerUnit, int quantity)
    {
        if (AmountPerUnit == amountPerUnit && Quantity == quantity)
        {
            return false;
        }

        AmountPerUnit = amountPerUnit;
        Quantity = quantity;
        
        return true;
    }
    #endregion
}