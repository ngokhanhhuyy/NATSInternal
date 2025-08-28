namespace NATSInternal.Application.UseCases;

public interface ISortableListRequestDto : IRequestDto
{
    #region Properties
    bool? SortByAscending { get; set; }
    string? SortByFieldName { get; set; }
    #endregion
}