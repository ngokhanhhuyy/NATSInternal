namespace NATSInternal.Services.Interfaces.Dtos;

public interface IListResponseDto<TBasic> where TBasic : IBasicResponseDto
{
    List<TBasic> Items { get; set; }
}