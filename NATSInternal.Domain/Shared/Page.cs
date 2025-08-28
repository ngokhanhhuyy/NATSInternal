namespace NATSInternal.Domain.Shared;

public class Page<TEntity> where TEntity : class
{
    #region Constructors
    public Page()
    {
        Items = new List<TEntity>().AsReadOnly();
    }
    public Page(List<TEntity> items, int pageCount)
    {
        Items = items.AsReadOnly();
        PageCount = pageCount;
    }
    #endregion

    #region Properties
    public IReadOnlyList<TEntity> Items { get; }
    public int PageCount { get; }
    #endregion
}
