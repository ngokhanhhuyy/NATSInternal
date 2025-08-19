namespace NATSInternal.Core.Interfaces.Dtos;

public interface ISortableListRequestDto : IRequestDto
{
    #region Properties
    bool? SortingByAscending { get; set; }
    string? SortingByFieldName { get; set; }
    #endregion
}