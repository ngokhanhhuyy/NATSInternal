namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICreatorTrackableListRequestDto : ISortableListRequestDto
{
    int? CreatedUserId { get; set; }
}