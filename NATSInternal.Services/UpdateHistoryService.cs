namespace NATSInternal.Services;

/// <inheritdoc />
internal class UpdateHistoryService<T, TUpdateHistory, TUpdateHistoryDataDto>
    : IUpdateHistoryService<T, TUpdateHistory, TUpdateHistoryDataDto>
    where T : class, IFinancialEngageableEntity<T, TUpdateHistory>, new()
    where TUpdateHistory : class, IUpdateHistoryEntity<TUpdateHistory>, new()
{
    private readonly IAuthorizationInternalService _authorizationService;

    public UpdateHistoryService(IAuthorizationInternalService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public void LogUpdateHistory(
            T entity,
            TUpdateHistoryDataDto oldData,
            TUpdateHistoryDataDto newData,
            string reason)
    {
        TUpdateHistory updateHistory = new TUpdateHistory
        {
            Reason = reason,
            OldData = JsonSerializer.Serialize(oldData),
            NewData = JsonSerializer.Serialize(newData),
            UpdatedDateTime = DateTime.UtcNow.ToApplicationTime(),
            UpdatedUserId = _authorizationService.GetUserId()
        };
        entity.UpdateHistories ??= new List<TUpdateHistory>();
        entity.UpdateHistories.Add(updateHistory);
    }
}