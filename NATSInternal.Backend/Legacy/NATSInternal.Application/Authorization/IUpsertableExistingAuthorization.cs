namespace NATSInternal.Application.Authorization;

public interface IUpsertableExistingAuthorizationResponseDto
{
    #region Properties
    bool CanEdit { get; set; }
    bool CanDelete { get; set; }
    #endregion
}