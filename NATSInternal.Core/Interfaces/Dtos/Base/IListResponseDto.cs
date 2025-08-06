namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IListResponseDto<TBasic> where TBasic : class, IBasicResponseDto
{
    int PageCount { get; set; }
    List<TBasic> Items { get; set; }
}