namespace NATSInternal.Models;

public class UserListModel : IListModel<UserBasicModel>
{
    public bool SortingByAscending { get; set; } = true;
    public string SortingByField { get; set; } = nameof(OrderByFieldOption.LastName);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public RoleMinimalModel Role { get; set; }
    public bool JoinedRencentlyOnly { get; set; } = false;
    public bool UpcomingBirthdayOnly { get; set; } = false;
    public string Content { get; set; }
    public int PageCount { get; set; }
    public List<UserBasicModel> Items { get; set; }
    public UserSecondaryListModel JoinedRecentlyUsers { get; set; }
    public UserSecondaryListModel UpcomingBirthdayUsers { get; set; }
    public List<RoleMinimalModel> RoleOptions { get; set; }
    public PaginationRangeModel PaginationRanges => new PaginationRangeModel(Page, PageCount);
    public bool CanCreate { get; set; }

    public void MapFromResponseDto(
            UserListResponseDto userListResponseDto,
            UserListResponseDto joinedRecentlyUsersResponseDto,
            UserListResponseDto incomingBirthdayUsersResponseDto,
            List<RoleMinimalResponseDto> roleOptionsResponseDtos)
    {
        PageCount = userListResponseDto.PageCount;
        Items = userListResponseDto.Items?
            .Select(u => new UserBasicModel(u))
            .ToList();
        JoinedRecentlyUsers = new UserSecondaryListModel(
            joinedRecentlyUsersResponseDto.Items,
            UserSecondaryListType.JoinedRecently);
        UpcomingBirthdayUsers = new UserSecondaryListModel(
            incomingBirthdayUsersResponseDto.Items,
            UserSecondaryListType.UpcomingBirthday);
        RoleOptions = roleOptionsResponseDtos
            .Select(dto => new RoleMinimalModel(dto))
            .ToList();
    }

    public UserListRequestDto ToRequestDto()
    {
        UserListRequestDto requestDto = new UserListRequestDto
        {
            SortingByField = SortingByField ?? nameof(OrderByFieldOption.LastName),
            SortingByAscending = SortingByAscending,
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