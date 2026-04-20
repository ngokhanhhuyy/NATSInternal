namespace NATSInternal.Core.Common.Services;

internal class Page<TResult>
{
    #region Constructors
    public Page()
    {
        Items = new List<TResult>().AsReadOnly();
    }
    public Page(List<TResult> items, int pageCount, int itemCount)
    {
        Items = items.AsReadOnly();
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion

    #region Properties
    public IReadOnlyList<TResult> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}
