namespace NATSInternal.Blazor.Models;

public class UserListModel : IListModel<UserBasicModel>
{
    public bool OrderByAscending { get; set; } = true;
    public string OrderByField { get; set; } =
        nameof(UserListRequestDto.FieldToBeOrdered.LastName);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public RoleBasicModel Role { get; set; }
    public bool JoinedRencentlyOnly { get; set; }
    public bool UpcomingBirthdayOnly { get; set; }
    public string Content { get; set; }
    public int PageCount { get; set; }
    public List<UserBasicModel> Items { get; set; }
    public List<UserBasicModel> JoinedRecentlyUsers { get; set; }
    public List<UserBasicModel> UpcomingBirthdayUsers { get; set; }
    public List<RoleBasicModel> RoleOptions { get; set; }
    public PaginationRangeModel PaginationRanges => new PaginationRangeModel(Page, PageCount);
    public UserListAuthorizationModel Authorization { get; set; }

    public void MapFromResponseDto(
            UserListResponseDto userListResponseDto,
            UserListResponseDto joinedRecentlyUsersResponseDto,
            UserListResponseDto incomingBirthdayUsersResponseDto,
            RoleListResponseDto roleOptionsResponseDto)
    {
        PageCount = userListResponseDto.PageCount;
        Items = userListResponseDto.Results?
            .Select(u => new UserBasicModel(u))
            .ToList();
        JoinedRecentlyUsers = joinedRecentlyUsersResponseDto.Results?
            .Select(responseDto => new UserBasicModel(responseDto))
            .ToList();
        UpcomingBirthdayUsers = incomingBirthdayUsersResponseDto.Results?
            .Select(responseDto => new UserBasicModel(responseDto))
            .ToList();
        RoleOptions = roleOptionsResponseDto.Items?
            .Select(responseDto => new RoleBasicModel(responseDto))
            .ToList();
        Authorization = new UserListAuthorizationModel(userListResponseDto.Authorization);
    }

    public UserListRequestDto ToRequestDto()
    {
        UserListRequestDto requestDto = new UserListRequestDto
        {
            OrderByField = OrderByField ??
                nameof(UserListRequestDto.FieldToBeOrdered.LastName),
            OrderByAscending = OrderByAscending,
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