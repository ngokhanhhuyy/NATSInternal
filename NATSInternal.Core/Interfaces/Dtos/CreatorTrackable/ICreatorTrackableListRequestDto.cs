namespace NATSInternal.Core.Interfaces.Dtos;

public interface ICreatorTrackableListRequestDto : ISortableAndPageableListRequestDto
{
    #region Properties
    Guid? CreatedUserId { get; set; }
    #endregion
}