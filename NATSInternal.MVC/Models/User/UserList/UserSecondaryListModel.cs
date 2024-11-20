using NATSInternal.Models;

namespace NATSInternal.MVC.Models;

public class UserSecondaryListModel
{
    public List<UserBasicModel> Items { get; set; }
    public UserSecondaryListType Type { get; set; }

    public UserSecondaryListModel(
            List<UserBasicResponseDto> responseDtos,
            UserSecondaryListType type)
    {
        Items = responseDtos.Select(dto => new UserBasicModel(dto)).ToList();
        Type = type;
    }
}