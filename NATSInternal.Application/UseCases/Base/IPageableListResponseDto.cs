namespace NATSInternal.Application.UseCases;

public interface IPageableListResponseDto<TItem>
{
    #region Properties
    IEnumerable<TItem> Items { get; }
    int PageCount { get; }
    int ItemCount { get; }
    #endregion
}