using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Authorization;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Common.Services;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;

namespace NATSInternal.Core.Features.Users;

internal class UserService : IUserService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IDbExceptionHandler _exceptionHandler;
    private readonly IValidator<UserCreateRequestDto> _createValidator;
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
        IValidator<UserCreateRequestDto> createValidator,
        IPasswordHasher passwordHasher,
        ICallerDetailProvider callerDetailProvider,
        IClock clock)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationService = authorizationService;
        _exceptionHandler = exceptionHandler;
        _createValidator = createValidator;
        _passwordHasher = passwordHasher;
        _callerDetailProvider = callerDetailProvider;
        _clock = clock;
    }
    #endregion
    
    #region Methods
    public async Task<UserListResponseDto> GetListAsync(UserListRequestDto requestDto)
    {
        IQueryable<User> query = _context.Users
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.DeletedDateTime != null)
            .Where(u => u.Roles.Select(r => r.Id).Any(rid => requestDto.RoleIds.Contains(rid)));

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
                query = query.ApplySorting(u => u.Roles.Max(r => r.PowerLevel), requestDto.SortByAscending);
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

    public async Task<UserDetailResponseDto> GetDetailAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .Where(u => u.Id == id)
            .Select(u => new UserDetailResponseDto(u, _authorizationService.GetUserExistingAuthorization(u)))
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
    }

    public async Task<int> CreateAsync(UserCreateRequestDto requestDto)
    {
        requestDto.TransformValues();
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
        }
    }
    #endregion
}