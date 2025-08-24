namespace NATSInternal.Core.Dtos;

public class DebtExistingAuthorizationResponseDto : IHasStatsExistingAuthorizationResponseDto
{
    #region Properties
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    public bool CanSetStatsDateTime { get; set; }
    #endregion
}