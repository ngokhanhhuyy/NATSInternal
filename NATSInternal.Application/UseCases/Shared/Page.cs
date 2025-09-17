namespace NATSInternal.Application.UseCases.Shared;

public class Page<TResult>
{
    #region Constructors
    public Page()
    {
        Items = new List<TResult>().AsReadOnly();
    }
    public Page(List<TResult> items, int pageCount)
    {
        Items = items.AsReadOnly();
        PageCount = pageCount;
    }
    #endregion

    #region Properties
    public IReadOnlyList<TResult> Items { get; }
    public int PageCount { get; }
    #endregion
}
