using NATSInternal.Domain.Exceptions;
using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Supplies;

internal class Supply : AbstractAggregateRootEntity, ITransactionEntity
{
    #region Fields
    private readonly List<SupplyItem> _items = new();
    #endregion

    #region Constructors
    #nullable disable
    private Supply() { }
    #nullable enable

    public Supply(
        long shipmentFee,
        string billCode,
        ICollection<SupplyItem> items,
        Guid createdUserId,
        DateTime createdDateTime)
    {
        ShipmentFee = shipmentFee;
        BillCode = billCode;
        _items = items.ToList();
        CreatedUserId = createdUserId;
        CreatedDateTime = createdDateTime;

        AddDomainEvent(new SupplyCreatedEvent(this));
    }
    #endregion

    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public long ShipmentFee { get; private set; }
    public string? BillCode { get; private set; }
    public DateTime TransactionDateTime { get; private set; }
    public DateTime CreatedDateTime { get; private set; }
    public DateTime? LastUpdatedDateTime { get; private set; }
    public DateTime? DeletedDateTime { get; private set; }
    #endregion

    #region ForeignKeyProperties
    public Guid CreatedUserId { get; private set; }
    public Guid? LastUpdatedUserId { get; private set; }
    public Guid? DeletedUserId { get; private set; }
    #endregion

    #region NavigationProperties
    public IReadOnlyList<SupplyItem> Items => _items.AsReadOnly();
    #endregion

    #region Methods
    public void Update(long shipmentFee, string billCode, Guid updatedUserId, DateTime updatedDateTime)
    {
        if (shipmentFee == ShipmentFee && billCode == BillCode)
        {
            return;
        }

        SupplyUpdatedEvent updatedEvent = GetUpdatedEvent(updatedDateTime);

        ShipmentFee = shipmentFee;
        BillCode = billCode;
        LastUpdatedUserId = updatedUserId;
        LastUpdatedDateTime = updatedDateTime;

        updatedEvent.AfterModificationSnapshot.ShipmentFee = shipmentFee;
        updatedEvent.AfterModificationSnapshot.BillCode = billCode;
    }

    public void Delete(Guid deletedUserId, DateTime deletedDateTime)
    {
        DeletedUserId = deletedUserId;
        DeletedDateTime = deletedDateTime;

        SupplyDeletedEvent deletedEvent = new(this, deletedDateTime);
        AddDomainEvent(deletedEvent);
    }

    public void AddItem(SupplyItem item, Guid updatedUserId, DateTime updatedDateTime)
    {
        if (_items.Any(i => i.Id == item.Id))
        {
            return;
        }

        SupplyUpdatedEvent updatedEvent = GetUpdatedEvent(updatedDateTime);

        _items.Add(item);
        LastUpdatedUserId = updatedUserId;
        LastUpdatedDateTime = updatedDateTime;

        updatedEvent.AfterModificationSnapshot.Items.Add(new(item));
    }

    public void UpdateItem(Guid itemId, long amountPerUnit, int quantity, Guid updatedUserId, DateTime updatedDateTime)
    {
        DomainException notFoundException = GenerateNotFoundException(itemId);

        SupplyItem? item = _items.SingleOrDefault(i => i.Id == itemId) ?? throw notFoundException;

        SupplyUpdatedEvent updatedEvent = GetUpdatedEvent(updatedDateTime);

        bool isItemUpdated = item.Update(amountPerUnit, quantity);
        if (!isItemUpdated)
        {
            return;
        }

        LastUpdatedUserId = updatedUserId;
        LastUpdatedDateTime = updatedDateTime;

        SupplyItemSnapshot itemSnapshot = updatedEvent.AfterModificationSnapshot.Items.Single(i => i.Id == itemId);
        itemSnapshot.AmountPerUnit = amountPerUnit;
        itemSnapshot.Quantity = quantity;
    }

    public void RemoveItem(Guid itemId, Guid updatedUserId, DateTime updatedDateTime)
    {
        DomainException notFoundException = GenerateNotFoundException(itemId);

        SupplyItem? item = _items.SingleOrDefault(i => i.Id == itemId) ?? throw notFoundException;

        SupplyUpdatedEvent updatedEvent = GetUpdatedEvent(updatedDateTime);

        bool removedSucceeded = _items.Remove(item);
        if (!removedSucceeded)
        {
            throw notFoundException;
        }

        LastUpdatedUserId = updatedUserId;
        LastUpdatedDateTime = updatedDateTime;

        SupplyItemSnapshot itemSnapshot = updatedEvent.AfterModificationSnapshot.Items.Single(i => i.Id == itemId);
        updatedEvent.AfterModificationSnapshot.Items.Remove(itemSnapshot);
    }
    #endregion

    #region PrivateMethods
    private SupplyUpdatedEvent GetUpdatedEvent(DateTime updatedDateTime)
    {
        SupplyUpdatedEvent? updatedEvent = DomainEvents.OfType<SupplyUpdatedEvent>().FirstOrDefault();
        if (updatedEvent is null)
        {
            updatedEvent = new(this, updatedDateTime);
            AddDomainEvent(updatedEvent);
        }

        return updatedEvent;
    }

    private DomainException GenerateNotFoundException(Guid itemId)
    {
        return new(
            $"{nameof(Supply)} entity with id \"{Id}\" doesn't " +
            $"contain any {nameof(SupplyItem)} entity with id \"{itemId}\"."
        );
    }
    #endregion
}