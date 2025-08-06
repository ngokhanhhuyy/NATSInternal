namespace NATSInternal.Core.Dtos;

public class NewCustomerCountResponseDto
{
    public int ThisMonth { get; set; }
    public int ThisYear { get; set; }
    public int ThisMonthCount { get; set; }
    public int PercentageComparedToLastMonth { get; set; }
}