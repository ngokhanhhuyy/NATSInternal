namespace NATSInternal.Services.Interfaces.Dtos;

public interface IListResponseDto<TBasic> where TBasic : IBasicResponseDto
{
    int PageCount { get; set; }
    List<TBasic> Items { get; set; }
}