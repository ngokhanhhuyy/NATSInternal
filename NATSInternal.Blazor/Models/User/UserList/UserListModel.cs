namespace NATSInternal.Blazor.Models;

public class UserListModel : IListModel<UserBasicModel>
{
    public bool SortingByAscending { get; set; }
    public string SortingByField { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public RoleMinimalModel Role { get; set; }
    public bool JoinedRencentlyOnly { get; set; }
    public bool UpcomingBirthdayOnly { get; set; }
    public string Content { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public List<UserBasicModel> Items { get; set; }
    public bool CanCreate { get; set; }
    public List<ListSortingByFieldModel> SortingByFieldOptions { get; set; }
    public List<RoleMinimalModel> RoleOptions { get; set; }
    public List<UserBasicModel> JoinedRecentlyUsers { get; set; }
    public List<UserBasicModel> UpcomingBirthdayUsers { get; set; }

    public UserListModel(
            UserListResponseDto listResponseDto,
            ListSortingOptionsResponseDto sortingOptionsResponseDto,
            List<RoleMinimalResponseDto> roleOptionsResponseDtos,
            List<UserBasicResponseDto> joinedRecentlyUserResponseDtos,
            List<UserBasicResponseDto> upcomingBirthdayUserResponseDtos,
            bool canCreate)
    {
        MapListResponseDto(listResponseDto);
        SortingByField = sortingOptionsResponseDto.DefaultFieldName;
        SortingByAscending = sortingOptionsResponseDto.DefaultAscending;
        SortingByFieldOptions = sortingOptionsResponseDto.FieldOptions
            .Select(option => new ListSortingByFieldModel(option))
            .ToList();
        RoleOptions = roleOptionsResponseDtos
            .Select(dto => new RoleMinimalModel(dto))
            .ToList();
        CanCreate = canCreate;
        JoinedRecentlyUsers = joinedRecentlyUserResponseDtos?
            .Select(dto => new UserBasicModel(dto))
            .ToList()
            ?? new List<UserBasicModel>();
        UpcomingBirthdayUsers = upcomingBirthdayUserResponseDtos?
            .Select(dto => new UserBasicModel(dto))
            .ToList()
            ?? new List<UserBasicModel>();
    }

    public void MapListResponseDto(UserListResponseDto responseDto)
    {
        PageCount = responseDto.PageCount;
        Items = responseDto.Items?
            .Select(dto => new UserBasicModel(dto))
            .ToList()
            ?? new List<UserBasicModel>();
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