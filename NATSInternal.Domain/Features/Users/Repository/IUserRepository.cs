using NATSInternal.Domain.Shared;

namespace NATSInternal.Domain.Features.Users;

public interface IUserRepository
{
    #region Methods
    Task<Page<User>> GetListWithRolesAsync(
        bool? sortByAscending,
        string? sortByFieldName,
        int? page,
        int? resultsPerPage,
        Guid? roleId,
        string? searchContent,
        CancellationToken cancellationToken = default);

    Task<User?> GetSingleByUserNameAsync(string userName, CancellationToken cancellationToken);

    Task<User?> GetSingleIncludedRolesWithPermissionsAsync(Guid id, CancellationToken cancellationToken = default);

    void Create(User user);

    void Update(User user);

    void Delete(User user);
    #endregion
}
