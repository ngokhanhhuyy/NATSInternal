namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service class to handle user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrives a list of user with basic information, based on the specified filtering, sorting and paginating
    /// conditions.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="UserListRequestDto"/> class, containing the conditions for the results.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is an instance of the
    /// <see cref="UserListResponseDto"/> class, containing the results and the additional information for pagination.
    /// </returns>
    Task<UserListResponseDto> GetListAsync(
        UserListRequestDto requestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of user with basic information, based on the specified ids.
    /// </summary>
    /// <param name="ids">
    /// An instance of the <see cref="IEnumerable{T}"/> implementation where <c>T</c> is <see cref="Guid"/>,
    /// representing the ids of the customers to retrieve.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="List{T}"/>>of the <see cref="UserListResponseDto"/> class instances, containing the results.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when there is any user with the specified id doesn't exist or has already been deleted.
    /// </exception>
    Task<List<UserBasicResponseDto>> GetMultipleAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a specific user's role details, specified by the user's id.
    /// </summary>
    /// <param name="id">
    /// The id of the user whose role is to be retrieved.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="RoleDetailResponseDto"/> class, containing the details of the role to retrieve.
    /// </returns>
    Task<RoleDetailResponseDto> GetRoleAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the basic information of a specific user, specified by the user's id.
    /// </summary>
    /// <param name="id">
    /// The id of the user to retrieve.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="UserBasicResponseDto"/> class, containing the basic information of the retrieving
    /// user.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the user with the specified <c>id</c> doesn't exist or has already been deleted.
    /// </exception>
    Task<UserBasicResponseDto> GetBasicAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get the details of a specific user, specified by the id of the user.
    /// </summary>
    /// <param name="id">
    /// An <see cref="int"/> representing the id of the user to retrieve.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// An instance of the <see cref="UserDetailResponseDto"/> class, containing the details of the user.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the user with the specified id doesn't exist or has already been deleted.
    /// </exception>
    Task<UserDetailResponseDto> GetDetailAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user, based on the specified data.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="UserCreateRequestDto"/> class, containing the data for the new user.
    /// </param>
    /// <returns>
    /// The id of the new user.
    /// </returns>
    /// <exception cref="DuplicatedException">
    /// Throws when the username specified in the argument for the
    /// <paramref name="requestDto"/> already exists.
    /// </exception>
    /// <exception cref="NotFoundException">
    /// Throws when the name of the role, specified by the value of the property <c>UserInformation.Role.Name</c> in the
    /// argument for the <paramref name="requestDto"/> doesn't exist.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to create a new user.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Throws when a business rules violation occurs during the assignment of the new user to the specified role.
    /// </exception>
    Task<Guid> CreateAsync(UserCreateRequestDto requestDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add an existing user, specified by id, to the roles specified by names.
    /// </summary>
    /// <param name="requestDto">
    /// A DTO containing the names of the roles to add.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    Task AddToRolesAsync(Guid id, AddToRolesRequestDto requestDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes the password of the requested user.
    /// </summary>
    /// <param name="requestDto">
    /// An instance of the <see cref="UserPasswordChangeRequestDto"/> class, contaning the current password, the new
    /// password and the confirmation password for the operation.
    /// </param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user isn't the target user.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the current password, provided in the <c>requestDto</c> is incorrect.
    /// </exception>
    Task ChangePasswordAsync(UserPasswordChangeRequestDto requestDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resets the password of the user, specified by the id, without the need of providing the current password.
    /// </summary>
    /// <param name="id">
    /// The id of the target user.
    /// </param>
    /// <param name="requestDto">
    /// An instance of the <see cref="UserPasswordResetRequestDto"/> class, contanining the new password and the
    /// confirmation password for the operation.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the user with the specified id doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user is actually the target user, or doesn't have enough permissions to reset the
    /// target user's password.
    /// </exception>
    /// <exception cref="OperationException">
    /// Throws when the specified new password's complexity doesn't meet the requirement.
    /// </exception>
    Task ResetPasswordAsync(
        Guid id,
        UserPasswordResetRequestDto requestDto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the user with the specified id.
    /// </summary>
    /// <param name="id">
    /// The id of the target user.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation.
    /// </returns>
    /// <exception cref="NotFoundException">
    /// Throws when the user with the specified id doesn't exist or has already been deleted.
    /// </exception>
    /// <exception cref="AuthorizationException">
    /// Throws when the requesting user doesn't have enough permissions to delete the target user.
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
    /// Check if the requesting user has permission to create a new user.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    bool GetCreatingPermission();

    /// <summary>
    /// Check if the requesting user has permission to create a new user.
    /// </summary>
    /// <param name="id">
    /// The id of the user to reset password.
    /// </param>
    /// <returns>
    /// <c>true</c> if the requesting user has the permission. Otherwise, <c>false</c>.
    /// </returns>
    Task<bool> GetPasswordResetPermission(Guid id, CancellationToken cancellationToken = default);
}
