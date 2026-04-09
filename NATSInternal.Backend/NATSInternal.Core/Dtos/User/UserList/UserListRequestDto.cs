namespace NATSInternal.Core.Dtos;

public class UserListRequestDto : ISortableAndPageableListRequestDto
{
    #region Properties
    public bool? SortByAscending { get; set; }
    public string? SortByFieldName { get; set; }
    public Guid? RoleId { get; set; }
    public string? SearchContent { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        SortByFieldName = SortByFieldName?.ToNullIfEmptyOrWhiteSpace();
        SearchContent = SearchContent?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        UserName,
        RoleMaxPowerLevel,
        CreatedDateTime
    }
    #endregion
}