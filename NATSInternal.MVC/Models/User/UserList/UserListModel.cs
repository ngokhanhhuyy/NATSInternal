namespace NATSInternal.Models;

public class UserListModel : ListModel<UserBasicModel>
{
    public override bool OrderByAscending { get; set; } = true;
    public RoleBasicModel Role { get; set; }
    public string Content { get; set; }
    public List<UserBasicModel> Results { get; set; }
    public List<RoleBasicModel> RoleOptions { get; set; }

    public UserListModel() { }

    public void MapFromResponseDto(
            UserListResponseDto responseDto,
            RoleListResponseDto roleListResponseDto)
    {
        PageCount = responseDto.PageCount;
        Results = responseDto.Results
            .Select(UserBasicModel.FromResponseDto)
            .ToList();
        RoleOptions = roleListResponseDto.Items
            .Select(RoleBasicModel.FromResponseDto)
            .ToList();
    }

    public UserListRequestDto ToRequestDto()
    {
        return new UserListRequestDto
        {
            OrderByField = OrderByField ??
                nameof(UserListRequestDto.FieldToBeOrdered.LastName),
            OrderByAscending = OrderByAscending,
            RoleId = Role?.Id,
            Content = Content,
            Page = Page,
        };
    }
}