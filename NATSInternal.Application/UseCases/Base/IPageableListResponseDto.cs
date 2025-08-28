namespace NATSInternal.Application.UseCases;

public interface IPageableListResponseDto<TItem>
{
    #region Properties
    ICollection<TItem> Items { get; }
    int PageCount { get; }
    #endregion
}