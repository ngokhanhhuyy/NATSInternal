namespace NATSInternal.Services.Dtos;

public class ListMonthYearRequestDto : IRequestDto
{
    public int Month { get; set; }
    public int Year { get; set; }

    public void TransformValues()
    {
    }
}
