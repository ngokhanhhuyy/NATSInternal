using NATSInternal.Domain.Features.Users;
using NATSInternal.Domain.Shared;

namespace NATSInternal.Domain.Repositories;

public interface IUserRepository
{
    #region Methods
    Task<Page<User>> GetListWithRolesAsync(
        bool? sortByAscending,
        string? sortByFieldName,
        Guid? roleId,
        string? searchContent,
        int? page,
        int? resultsPerPage,
        CancellationToken cancellationToken = default);

    Task<User?> GetSingleIncludedRolesWithPermissionsAsync(Guid id, CancellationToken cancellationToken = default);

    void Create(User user);

    void Update(User user);

    void Delete(User user);
    #endregion
}
