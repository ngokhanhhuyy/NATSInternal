namespace NATSInternal.Core.Features.Authorization;

public interface IUpsertableExistingAuthorizationResponseDto
{
    #region Properties
    bool CanEdit { get; set; }
    bool CanDelete { get; set; }
    #endregion
}