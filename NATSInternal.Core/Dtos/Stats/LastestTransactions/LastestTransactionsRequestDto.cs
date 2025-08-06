namespace NATSInternal.Core.Dtos;

public class LatestTransactionsRequestDto : IRequestDto
{
    public int Count { get; set; } = 5;
}