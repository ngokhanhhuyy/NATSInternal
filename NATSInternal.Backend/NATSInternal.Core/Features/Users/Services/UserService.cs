using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Services;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace NATSInternal.Core.Features.Users;

internal class UserService : IUserService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly IValidator<UserListRequestDto> _listValidator;
    private readonly IValidator<UserCreateRequestDto> _createValidator;
    private readonly IValidator<UserUpdateRequestDto> _updateValidator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IClock _clock;
    #endregion
    
    #region Constructors
    public UserService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationService,
        IDbExceptionHandler exceptionHandler,
        IValidator<UserListRequestDto> listValidator,
        IValidator<UserCreateRequestDto> createValidator,
        IValidator<UserUpdateRequestDto> updateValidator,
        IPasswordHasher passwordHasher,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationService = authorizationService;
        _exceptionHandler = exceptionHandler;
        _listValidator = listValidator;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _passwordHasher = passwordHasher;
        _callerDetailProvider = callerDetailProvider;
        _clock = clock;
    }
    #endregion
    
    #region Methods
    public async Task<UserListResponseDto> GetListAsync(UserListRequestDto requestDto)
    {
        _listValidator.ValidateAndThrow(requestDto);

        IQueryable<User> query = _context.Users
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.DeletedDateTime == null);

        if (requestDto.RoleIds.Count > 0)
        {
            query = query.Where(u => u.Roles.Select(r => r.Id).Any(rid => requestDto.RoleIds.Contains(rid)));
        }

        if (!string.IsNullOrEmpty(requestDto.SearchContent))
        {
            query = query.Where(u => requestDto.SearchContent.ToLower().Contains(u.UserName.ToLower()));
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(UserListRequestDto.FieldToSort.UserName):
                query = query.ApplySorting(u => u.UserName, requestDto.SortByAscending);
                break;
            case nameof(UserListRequestDto.FieldToSort.CreatedDateTime):
                query = query.ApplySorting(u => u.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(UserListRequestDto.FieldToSort.RoleMaxPowerLevel):
                query = query
                    .ApplySorting(u => u.Roles.Max(r => r.PowerLevel), requestDto.SortByAscending)
                    .ThenApplySorting(u => u.UserName, requestDto.SortByAscending);
                break;
            default:
                throw new NotImplementedException();
        }

        Page<User> queryResult = await _listFetchingService.GetPagedListAsync(
            query,
            requestDto.Page,
            requestDto.ResultsPerPage
        );

        List<UserBasicResponseDto> userResponseDtos = queryResult.Items
            .Select(u => new UserBasicResponseDto(u, _authorizationService.GetUserExistingAuthorization(u)))
            .ToList();

        return new(userResponseDtos, queryResult.PageCount, queryResult.ItemCount);
    }

    public async Task<UserDetailResponseDto> GetDetailByIdAsync(int id)
    {
        User user = await GetUserWithRelatedEntities(u => u.Id == id) ?? throw new NotFoundException();
            return new(user, _authorizationService.GetUserExistingAuthorization(user));
    }

    public async Task<UserDetailResponseDto> GetDetailByUserNameAsync(
        string userName,
        bool includingAuthorization = false)
    {
        User user = await GetUserWithRelatedEntities(u => u.UserName == userName) ?? throw new NotFoundException();
            
        if (includingAuthorization)
        {
            return new(user, _authorizationService.GetUserExistingAuthorization(user));
        }

        return new(user);
    }

    public async Task<int> CreateAsync(UserCreateRequestDto requestDto)
    {
        _createValidator.ValidateAndThrow(requestDto);

        if (!_authorizationService.CanCreateUser())
        {
            throw new AuthorizationException();
        }

        string passwordHash = _passwordHasher.HashPassword(requestDto.Password);
        User user = new()
        {
            UserName = requestDto.UserName,
            PasswordHash = passwordHash,
            CreatedDateTime = _clock.Now,
            CreatedUserId = _callerDetailProvider.GetId()
        };

        _context.Users.Add(user);

        List<Role> existingRoles = await _context.Roles
            .Where(r => requestDto.RoleNames.Contains(r.Name))
            .ToListAsync();

        List<string> existingRoleNames = existingRoles.Select(r => r.Name).ToList();

        for (int index = 0; index < requestDto.RoleNames.Count; index += 1)
        {
            string requestedRoleName = requestDto.RoleNames[index];
            if (!existingRoleNames.Contains(requestedRoleName))
            {
                throw OperationException.NotFound(
                    new object[] { nameof(requestDto.RoleNames), index },
                    DisplayNames.Role
                );
            }

            Role existingRole = existingRoles[index];
            user.Roles.Add(existingRole);
        }

        try
        {
            await _context.SaveChangesAsync();
            return user.Id;
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsUniqueConstraintViolation)
            {
                throw OperationException.Duplicated(
                    new object[] { nameof(requestDto.UserName) },
                    DisplayNames.UserName
                );
            }

            throw;
        }
    }

    public async Task UpdateAsync(int id, UserUpdateRequestDto requestDto)
    {
        _updateValidator.ValidateAndThrow(requestDto);
        
        User user = await _context.Users
            .Include(u => u.Roles)
            .Where(u => u.Id == id && u.DeletedDateTime == null)
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    
        List<int> alreadyAddedRoleIds = user.Roles.Select(u => u.Id).ToList();
        List<Role> availableRoles = await _context.Roles.ToListAsync();
        List<int> availableRoleIds = availableRoles.Select(r => r.Id).ToList();

        for (int index = 0; index < requestDto.RoleIds.Count; index += 1)
        {
            if (alreadyAddedRoleIds.Contains(requestDto.RoleIds[index]))
            {
                continue;
            }

            Role requestedRole = availableRoles
                .SingleOrDefault(r => r.Id == requestDto.RoleIds[index])
                ?? throw OperationException.NotFound(
                    new object[] { nameof(requestDto.RoleIds), index },
                    DisplayNames.Role
                );

            if (!_authorizationService.CanAddUserToRole(user, requestedRole))
            {
                throw new AuthorizationException();
            }

            user.Roles.Add(requestedRole);
        }
    
        List<Role> rolesToBeDeleted = user.Roles.Where(r => !requestDto.RoleIds.Contains(r.Id)).ToList();
        foreach (Role role in rolesToBeDeleted)
        {
            user.Roles.Remove(role);
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            if (handledResult.IsForeignKeyConstraintViolation)
            {
                throw OperationException.NotFound(new object[] { nameof(requestDto.RoleIds) }, DisplayNames.Role);
            }

            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        User user = await _context.Users
            .SingleOrDefaultAsync(u => u.Id == id && u.DeletedDateTime == null)
            ?? throw new NotFoundException();

        if (!_authorizationService.CanDeleteUser(user))
        {
            throw new AuthorizationException();
        }

        _context.Users.Remove(user);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }

    public async Task RestoreAsync(int id)
    {
        User user = await _context.Users
            .SingleOrDefaultAsync(u => u.Id == id && u.DeletedDateTime != null)
            ?? throw new NotFoundException();

        if (!_authorizationService.CanRestoreUser(user))
        {
            throw new AuthorizationException();
        }

        _context.Users.Remove(user);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
        {
            DbExceptionHandledResult? handledResult = _exceptionHandler.Handle(exception);
            if (handledResult is null)
            {
                throw;
            }

            if (handledResult.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }
    #endregion

    #region PrivateMethods
    private async Task<User?> GetUserWithRelatedEntities(Expression<Func<User, bool>> conditionExpression)
    {
        IIncludableQueryable<User, List<Role>> query = _context.Users
            .AsSingleQuery()
            .Where(u => u.DeletedDateTime == null)
            .Include(u => u.Roles);

        User user = await query
            .ThenInclude(r => r.Permissions)
            .Where(conditionExpression)
            .AsNoTracking()
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();

        if (user.CreatedUserId.HasValue)
        {
            user.CreatedUser = await query.Where(u => u.Id == user.CreatedUserId.Value).SingleAsync();
        }

        if (user.LastUpdatedUserId.HasValue)
        {
            user.LastUpdatedUser = await query.Where(u => u.Id == user.LastUpdatedUserId.Value).SingleAsync();
        }

        if (user.DeletedUserId.HasValue)
        {
            user.DeletedUser = await query.Where(u => u.Id == user.DeletedUserId.Value).SingleAsync();
        }

        return user;
    }
    #endregion
}