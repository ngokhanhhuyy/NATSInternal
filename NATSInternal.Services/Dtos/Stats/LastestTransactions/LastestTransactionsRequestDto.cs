namespace NATSInternal.Services.Dtos;

public class LatestTransactionsRequestDto : IRequestDto
{
    public int Count { get; set; } = 5;
}