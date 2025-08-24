namespace NATSInternal.Core.Interfaces.Dtos;

public interface ICreatorTrackableListRequestDto : ISortableAndPageableListRequestDto
{
    int? CreatedUserId { get; set; }
}