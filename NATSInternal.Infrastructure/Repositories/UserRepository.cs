using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Users;
using NATSInternal.Domain.Repositories;
using NATSInternal.Domain.Shared;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Infrastructure.Extensions;

namespace NATSInternal.Infrastructure.Repositories;

internal class UserRepository : IUserRepository
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly ListRepository _listRepository;
    #endregion

    #region Constructors
    public UserRepository(AppDbContext context, ListRepository listRepository)
    {
        _context = context;
        _listRepository = listRepository;
    }
    #endregion

    #region Methods
    public async Task<Page<User>> GetListWithRolesAsync(
        bool? sortByAscending,
        string? sortByFieldName,
        Guid? roleId,
        string? searchContent,
        int? page,
        int? resultsPerPage,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = _context.Users.Include(u => u.Roles);

        if (roleId.HasValue)
        {
            query = query.Where(u => u.Roles.Select(r => r.Id).Contains(roleId.Value));
        }

        if (searchContent is not null)
        {
            query = query.Where(u => u.NormalizedUserName.Contains(searchContent.ToUpper()));
        }

        bool sortByAscendingOrDefault = sortByAscending ?? true;
        switch (sortByFieldName)
        {
            case nameof(User.UserName) or null:
                query = query.ApplySorting(u => u.UserName, sortByAscendingOrDefault);
                break;
            case nameof(User.PowerLevel):
                query = query.ApplySorting(u => u.Roles.Max(r => r.PowerLevel), sortByAscendingOrDefault);
                break;
            case nameof(User.CreatedDateTime):
                query = query.ApplySorting(u => u.CreatedDateTime, sortByAscendingOrDefault);
                break;
            default:
                throw new NotImplementedException();
        }

        return await _listRepository.GetPagedListAsync(query, page, resultsPerPage, cancellationToken);
    }

    public async Task<User?> GetSingleIncludedRolesWithPermissionsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public void Create(User user)
    {
        _context.Users.Add(user);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        _context.Users.Remove(user);
    }
    #endregion
}
