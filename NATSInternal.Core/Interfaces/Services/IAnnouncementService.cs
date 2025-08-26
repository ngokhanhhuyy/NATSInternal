namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle announcements.
/// </summary>
public interface IAnnouncementService
{
    #region Methods
    /// <summary>
    /// Retrieves a list of announcements, based on the specified filtering, sorting and paginating conditions.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="AnnouncementListRequestDto"/> class, contaning the conditions for the results.
    /// </param>
    /// <param name="cancellationToken">
    /// (Optional) A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="AnnouncementListResponseDto"/> class, containing the results and the additional
    /// information for pagination.
    /// </returns>
    Task<AnnouncementListResponseDto> GetListAsync(
            AnnouncementListRequestDto requestDto,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the details of a specific announcement, based on its id.
    /// </summary>
    /// <param name="id">
    /// A <see cref="Guid"/> representing the id of the announcement to retrieve.
    /// </param>
    /// <param name="cancellationToken">
    /// (Optional) A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="AnnouncementResponseDto"/> class, containing the details of the announcement.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the announcement with the specified doesn't exist or has already been deleted.
    /// </exception>
    Task<AnnouncementResponseDto> GetDetailAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new announcement based on the specified data.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="AnnouncementUpsertRequestDto"/> class, containing the data for the creating
    /// operation.
    /// </param>
    /// <param name="cancellationToken">
    /// (Optional) A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A <see cref="Guid"/> represeting the id of the new announcement.
    /// </returns>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    Task<Guid> CreateAsync(AnnouncementUpsertRequestDto requestDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing announcement based on its id and the specified data.
    /// </summary>
    /// <param name="id">
    /// A <see cref="Guid"/> representing the id of the announcement to update.
    /// </param>
    /// <param name="requestDto">
    /// An instance of the <see cref="AnnouncementUpsertRequestDto"/> class, containing the data for the updating
    /// operation.
    /// </param>
    /// <param name="cancellationToken">
    /// (Optional) A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <exception cref="NotFoundException">
    /// Throws when the announcement with the specified id doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Throws when a concurrency-related conflict occurs during the operation.
    /// </exception>
    Task UpdateAsync(Guid id, AnnouncementUpsertRequestDto requestDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing announcement based on its id.
    /// </summary>
    /// <param name="id">
    /// An <see cref="Guid"/> representing the id of the announcement to delete.
    /// </param>
    /// <param name="cancellationToken">
    /// (Optional) A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <exception cref="NotFoundException">
    /// Throws when the announcement with the specified id doesn't exist or has already been deleted.
    /// </exception>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all fields those are used as options to order the results in list retrieving operation.
    /// </summary>
    /// <returns>
    /// An instance of the <see cref="ListSortingOptionsResponseDto"/> DTO, containing the options with name and display
    /// names of the fields and the default field.
    /// </returns>
    ListSortingOptionsResponseDto GetListSortingOptions();

    /// <summary>
    /// Check if the requesting user has permission to create a new announcement.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();
    #endregion
}
