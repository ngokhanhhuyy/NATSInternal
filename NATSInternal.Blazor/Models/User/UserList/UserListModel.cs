namespace NATSInternal.Blazor.Models;

public class UserListModel : IListModel<UserBasicModel>
{
    public bool SortingByAscending { get; set; } = true;
    public string SortingByField { get; set; } = nameof(OrderByFieldOption.LastName);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public RoleMinimalModel Role { get; set; }
    public bool JoinedRencentlyOnly { get; set; }
    public bool UpcomingBirthdayOnly { get; set; }
    public string Content { get; set; }
    public int PageCount { get; set; }
    public List<UserBasicModel> Items { get; set; }
    public bool CanCreate { get; set; }

    public void MapFromResponseDto(UserListResponseDto userListResponseDto, bool canCreate)
    {
        PageCount = userListResponseDto.PageCount;
        Items = userListResponseDto.Items?
            .Select(u => new UserBasicModel(u))
            .ToList()
            ?? new List<UserBasicModel>();
        CanCreate = canCreate;
    }

    public UserListRequestDto ToRequestDto()
    {
        UserListRequestDto requestDto = new UserListRequestDto
        {
            SortingByAscending = SortingByAscending,
            SortingByField = SortingByField,
            RoleId = Role?.Id,
            JoinedRencentlyOnly = JoinedRencentlyOnly,
            UpcomingBirthdayOnly = UpcomingBirthdayOnly,
            Content = Content,
            Page = Page,
        };

        requestDto.TransformValues();
        return requestDto;
    }
}