namespace NATSInternal.Core.Dtos;

public class DebtIncurrenceExistingAuthorizationResponseDto
    : IHasStatsExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSetIncurredDateTime { get; set; }

    public bool CanSetStatsDateTime
    {
        get => CanSetIncurredDateTime;
        set => CanSetIncurredDateTime = value;
    }
}