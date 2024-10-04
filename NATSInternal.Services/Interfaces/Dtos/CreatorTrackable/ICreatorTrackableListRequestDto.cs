namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICreatorTrackableListRequestDto<TRequestDto> : IListRequestDto<TRequestDto>
{
    int? CreatedUserId { get; set; }
}