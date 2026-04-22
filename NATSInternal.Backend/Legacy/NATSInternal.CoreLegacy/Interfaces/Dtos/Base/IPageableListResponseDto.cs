namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IPageableListResponseDto<TBasic> where TBasic : class, IBasicResponseDto
{
    #region Properties
    int PageCount { get; set; }
    List<TBasic> Items { get; set; }
    #endregion
}