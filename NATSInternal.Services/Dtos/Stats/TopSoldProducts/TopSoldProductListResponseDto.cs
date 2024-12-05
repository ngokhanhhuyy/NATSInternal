namespace NATSInternal.Services.Dtos;

public class TopSoldProductListResponseDto
{
    public required DateOnly StartingDate { get; set; }
    public required DateOnly EndingDate { get; set; }
    public required List<TopSoldProductResponseDto> Items { get; set; }
}
