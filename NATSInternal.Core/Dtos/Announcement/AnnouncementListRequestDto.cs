namespace NATSInternal.Core.Dtos;

public class AnnouncementListRequestDto : ISortableAndPageableListRequestDto
{
    #region Properties
    public bool? SortByAscending { get; set; }
    public string? SortByFieldName { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
    
    #region Enums
    public enum FieldToSort
    {
        StartingDateTime
    }
    #endregion
}