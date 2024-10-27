namespace NATSInternal.Blazor.Models;

public class UserListModel : IListModel<UserBasicModel>
{
    public bool OrderByAscending { get; set; } = true;
    public string OrderByField { get; set; } = nameof(OrderByFieldOptions.LastName);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public RoleBasicModel Role { get; set; }
    public bool JoinedRencentlyOnly { get; set; }
    public bool UpcomingBirthdayOnly { get; set; }
    public string Content { get; set; }
    public int PageCount { get; set; }
    public List<UserBasicModel> Items { get; set; }
    public UserListAuthorizationModel Authorization { get; set; }

    public void MapFromResponseDto(UserListResponseDto userListResponseDto)
    {
        PageCount = userListResponseDto.PageCount;
        Items = userListResponseDto.Results?
            .Select(u => new UserBasicModel(u))
            .ToList()
            ?? new List<UserBasicModel>();
        Authorization = new UserListAuthorizationModel(userListResponseDto.Authorization);
    }

    public UserListRequestDto ToRequestDto()
    {
        UserListRequestDto requestDto = new UserListRequestDto
        {
            OrderByField = OrderByField,
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