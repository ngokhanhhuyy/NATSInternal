namespace NATSInternal.Core.Interfaces.Dtos;

public interface ISortableListRequestDto : IRequestDto
{
    #region Properties
    bool? SortByAscending { get; set; }
    string? SortByFieldName { get; set; }
    #endregion
}