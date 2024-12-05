namespace NATSInternal.Services.Dtos;

public class LastestTransactionsRequestDto : IRequestDto
{
    public int Count { get; set; } = 5;
}