using NATSInternal.Domain.Shared;

namespace NATSInternal.Domain.Features.Users;

internal interface IUserRepository
{
    #region Methods
    Task<Page<User>> GetUserListAsync(
        bool? sortByAscending,
        string? sortByFieldName,
        int? page,
        int? resultsPerPage,
        Guid? roleId,
        string? searchContent,
        CancellationToken cancellationToken = default);

    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);

    void AddUser(User user);

    void UpdateUser(User user);

    void DeleteUser(User user);

    Task<List<Role>> GetRolesByNameAsync(IEnumerable<string> roleNames, CancellationToken cancellationToken = default);
    #endregion
}
