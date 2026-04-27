namespace NATSInternal.Core.Features.Users;

/// <summary>
/// Defines operations for listing, retrieving, creating, updating, deleting, and restoring users.
/// </summary>
public interface IUserService
{
    #region Methods
    /// <summary>
    /// Retrieves a paginated list of active users that match the supplied filter and sort criteria.
    /// </summary>
    /// <param name="requestDto">
    /// The request containing role filters, search text, sort options, page number, and page size.
    /// </param>
    /// <returns>
    /// A task that returns the paginated user list response with items, page count, and total item count.
    /// </returns>
    /// <exception cref="FluentValidation.ValidationException">
    /// Thrown when <paramref name="requestDto"/> fails validation.
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Thrown when the requested sort field is not implemented by the service.
    /// </exception>
    /// <example>
    /// <code>
    /// UserListResponseDto users = await userService.GetListAsync(new UserListRequestDto
    /// {
    ///     SearchContent = "admin",
    ///     SortByFieldName = nameof(UserListRequestDto.FieldToSort.UserName),
    ///     SortByAscending = true,
    ///     Page = 1,
    ///     ResultsPerPage = 20
    /// });
    /// </code>
    /// </example>
    Task<UserListResponseDto> GetListAsync(UserListRequestDto requestDto);

    /// <summary>
    /// Retrieves detailed information for a user by identifier.
    /// </summary>
    /// <param name="id">The identifier of the user to retrieve.</param>
    /// <returns>
    /// A task that returns the detailed user response for the matching user.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no user exists for <paramref name="id"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// UserDetailResponseDto user = await userService.GetDetailByIdAsync(7);
    /// </code>
    /// </example>
    Task<UserDetailResponseDto> GetDetailByIdAsync(int id);

    /// <summary>
    /// Retrieves detailed information for a user by username.
    /// </summary>
    /// <param name="userName">The username of the user to retrieve.</param>
    /// <param name="includingAuthorization">
    /// <see langword="true"/> to include the current caller's authorization for the user; otherwise,
    /// <see langword="false"/>.
    /// </param>
    /// <returns>
    /// A task that returns the detailed user response for the matching username.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no user exists for <paramref name="userName"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// UserDetailResponseDto user = await userService.GetDetailByUserNameAsync("admin", true);
    /// </code>
    /// </example>
    Task<UserDetailResponseDto> GetDetailByUserNameAsync(string userName, bool includingAuthorization = false);

    /// <summary>
    /// Creates a new user from the supplied data and returns the created user identifier.
    /// </summary>
    /// <param name="requestDto">
    /// The request containing the username, password, and roles to assign.
    /// </param>
    /// <returns>
    /// A task that returns the identifier assigned to the newly created user.
    /// </returns>
    /// <exception cref="FluentValidation.ValidationException">
    /// Thrown when <paramref name="requestDto"/> fails validation.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user is not allowed to create users.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when one of the requested roles does not exist.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when another user already uses the supplied username.
    /// </exception>
    /// <example>
    /// <code>
    /// int userId = await userService.CreateAsync(new UserCreateRequestDto
    /// {
    ///     UserName = "jane.doe",
    ///     Password = "P@ssw0rd!",
    ///     RoleNames = ["Manager"]
    /// });
    /// </code>
    /// </example>
    Task<int> CreateAsync(UserCreateRequestDto requestDto);

    /// <summary>
    /// Updates the role assignments of an existing active user.
    /// </summary>
    /// <param name="id">The identifier of the user to update.</param>
    /// <param name="requestDto">
    /// The request containing the role identifiers that the user should have after the update.
    /// </param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    /// <exception cref="FluentValidation.ValidationException">
    /// Thrown when <paramref name="requestDto"/> fails validation.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Thrown when no active user exists for <paramref name="id"/>.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user is not allowed to assign one of the requested roles.
    /// </exception>
    /// <exception cref="OperationException">
    /// Thrown when one of the requested roles does not exist.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a concurrency conflict occurs while saving the user changes.
    /// </exception>
    /// <example>
    /// <code>
    /// await userService.UpdateAsync(7, new UserUpdateRequestDto
    /// {
    ///     RoleIds = [1, 3]
    /// });
    /// </code>
    /// </example>
    Task UpdateAsync(int id, UserUpdateRequestDto requestDto);

    /// <summary>
    /// Deletes an existing active user.
    /// </summary>
    /// <param name="id">The identifier of the user to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no active user exists for <paramref name="id"/>.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user is not allowed to delete the specified user.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a concurrency conflict occurs while saving the delete operation.
    /// </exception>
    /// <example>
    /// <code>
    /// await userService.DeleteAsync(7);
    /// </code>
    /// </example>
    Task DeleteAsync(int id);

    /// <summary>
    /// Restores a deleted user identified by <paramref name="id"/>.
    /// </summary>
    /// <param name="id">The identifier of the deleted user to restore.</param>
    /// <returns>A task that represents the asynchronous restore operation.</returns>
    /// <exception cref="NotFoundException">
    /// Thrown when no deleted user exists for <paramref name="id"/>.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Thrown when the current user is not allowed to restore the specified user.
    /// </exception>
    /// <exception cref="ConcurrencyException">
    /// Thrown when a concurrency conflict occurs while saving the restore operation.
    /// </exception>
    /// <example>
    /// <code>
    /// await userService.RestoreAsync(7);
    /// </code>
    /// </example>
    Task RestoreAsync(int id);
    #endregion
}
