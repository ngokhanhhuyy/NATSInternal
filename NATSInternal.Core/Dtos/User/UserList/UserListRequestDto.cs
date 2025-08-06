namespace NATSInternal.Core.Dtos;

public class UserListRequestDto : IRequestDto
{
    public bool? SortingByAscending { get; set; }
    public string SortingByField { get; set; }
    public int? RoleId { get; set; }
    public bool JoinedRencentlyOnly { get; set; }
    public bool UpcomingBirthdayOnly { get; set; }
    public string Content { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
        SortingByField = SortingByField?.ToNullIfEmpty();
        Content = Content?.ToNullIfEmpty();
    }
}