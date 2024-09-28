namespace NATSInternal.Services.Dtos;

internal interface IListResponseDto<TBasicResponseDto>
{
    int PageCount { get; set; }
    List<TBasicResponseDto> Items { get; set; }
}