namespace NATSInternal.Core.Interfaces.Dtos;

public interface ICreatorTrackableListRequestDto : ISortableListRequestDto
{
    int? CreatedUserId { get; set; }
}