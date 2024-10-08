namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICreatorTrackableListRequestDto : IListRequestDto
{
    int? CreatedUserId { get; set; }
}