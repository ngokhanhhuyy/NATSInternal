namespace NATSInternal.Core.Common.Dtos;

public interface IListResponseDto<TBasic> where TBasic : class
{
    #region Properties
    List<TBasic> Items { get; set; }
    int PageCount { get; set; }
    int ItemCount { get; set; }
    #endregion
}