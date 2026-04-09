namespace NATSInternal.Core.Dtos;

public class DebtCreatingAuthorizationResponseDto : IHasStatsCreatingAuthorizationResponseDto
{
    #region Properties
    public bool CanSetStatsDateTime { get; set; }
    #endregion
}