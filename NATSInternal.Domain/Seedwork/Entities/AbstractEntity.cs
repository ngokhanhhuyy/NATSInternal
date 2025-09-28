namespace NATSInternal.Domain.Seedwork;

public abstract class AbstractEntity
{
    #region Fields
    private readonly List<IDomainEvent> _domainEvents = new();
    #endregion

    #region Properties
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    #endregion

    #region Methods
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    #endregion
    
    #region ProtectedMethods
    protected void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }
    #endregion
}
