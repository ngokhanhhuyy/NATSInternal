namespace NATSInternal.Core.Dtos;

public class UserListRequestDto : IRequestDto
{
    #region Properties
    public bool? SortingByAscending { get; set; }
    public string? SortingByField { get; set; }
    public Guid? RoleId { get; set; }
    public bool? JoinedRencentlyOnly { get; set; }
    public bool? UpcomingBirthdayOnly { get; set; }
    public string? Content { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        SortingByField = SortingByField?.ToNullIfEmpty();
        Content = Content?.ToNullIfEmpty();
    }
    #endregion
}