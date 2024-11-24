namespace NATSInternal.Blazor.Models;

public class ListSortingByFieldModel
{
    public string Name { get; set; }
    public string DisplayName { get; set; }

    public ListSortingByFieldModel(ListSortingByFieldResponseDto responseDto)
    {
        Name = responseDto.Name;
        DisplayName = responseDto.DisplayName;
    }
}