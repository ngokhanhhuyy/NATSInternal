namespace NATSInternal.Application.UseCases;

public interface IListResponseDto<out TItemResponseDto>
{
    #region Properties
    IEnumerable<TItemResponseDto> Items { get; }
    int PageCount { get; }
    int ItemCount { get; }
    #endregion
}