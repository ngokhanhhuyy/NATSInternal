namespace NATSInternal.Core.Common.Dtos;

public interface IListResponseDto<TBasic> where TBasic : class
{
    #region Properties
    List<TBasic> Items { get; }
    int PageCount { get; }
    int ItemCount { get; }
    #endregion
}