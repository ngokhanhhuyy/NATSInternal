namespace NATSInternal.Core.Dtos;

public class ListSortingOptionsResponseDto
{
    public required List<ListSortingByFieldResponseDto> FieldOptions { get; set; }
    public required string DefaultFieldName { get; set; }
    public required bool DefaultAscending { get; set; }
}