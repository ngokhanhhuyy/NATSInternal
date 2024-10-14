namespace NATSInternal.Services.Dtos;

public class ExpenseListAuthorizationResponseDto : IUpsertableListAuthorizationResponseDto
{
    public bool CanCreate { get; set; }
}
