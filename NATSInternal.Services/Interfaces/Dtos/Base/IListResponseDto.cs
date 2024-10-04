namespace NATSInternal.Services.Interfaces.Dtos;

public interface IListResponseDto<TBasic> where TBasic : IBasicResponseDto
{
    int PageCount { get; internal set; }
    List<TBasic> Items { get; internal set; }
}