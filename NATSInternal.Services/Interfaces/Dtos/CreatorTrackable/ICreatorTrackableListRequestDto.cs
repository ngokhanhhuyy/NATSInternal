namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICreatorTrackableListRequestDto : IOrderableListRequestDto
{
    int? CreatedUserId { get; set; }
}