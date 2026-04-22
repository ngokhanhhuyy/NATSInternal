namespace NATSInternal.Domain.Features.Users;

internal interface IUserRepository
{
    #region Methods
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<User?> GetUserByUserNameAsync(string userName, CancellationToken cancellationToken);

    void AddUser(User user);

    void UpdateUser(User user);

    Task<List<Role>> GetRolesByNameAsync(IEnumerable<string> roleNames, CancellationToken cancellationToken = default);
    #endregion
}
