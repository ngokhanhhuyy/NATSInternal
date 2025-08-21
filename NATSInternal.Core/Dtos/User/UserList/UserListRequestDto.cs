namespace NATSInternal.Core.Dtos;

public class UserListRequestDto : ISortableAndPageableListRequestDto
{
    #region Properties
    public bool? SortingByAscending { get; set; }
    public string? SortingByFieldName { get; set; }
    public Guid? RoleId { get; set; }
    public string? SearchContent { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        SortingByFieldName = SortingByFieldName?.ToNullIfEmptyOrWhiteSpace();
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