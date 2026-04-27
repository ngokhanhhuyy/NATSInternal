using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Users;
using NATSInternal.Core.Persistence.DbContext;
using NATSInternal.Core.Persistence.Handlers;

namespace NATSInternal.Core.Features.Authentication;

internal class AuthenticationService : IAuthenticationService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IAuthorizationInternalService _authorizationService;
    private readonly IValidator<VerifyUserNameAndPasswordRequestDto> _verifyUserNameAndPasswordValidator;
    private readonly IValidator<ChangePasswordRequestDto> _changePasswordValidator;
    private readonly IValidator<ResetPasswordRequestDto> _resetPasswordValidator;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDbExceptionHandler _exceptionHandler;
    #endregion
    
    #region Constructors
    public AuthenticationService(
        AppDbContext context,
        IAuthorizationInternalService authorizationService,
        IValidator<VerifyUserNameAndPasswordRequestDto> verifyUserNameAndPasswordValidator,
        IValidator<ChangePasswordRequestDto> changePasswordValidator,
        IValidator<ResetPasswordRequestDto> resetPasswordValidator,
        ICallerDetailProvider callerDetailProvider,
        IPasswordHasher passwordHasher,
        IDbExceptionHandler exceptionHandler)
    {
        _context = context;
        _authorizationService = authorizationService;
        _verifyUserNameAndPasswordValidator = verifyUserNameAndPasswordValidator;
        _changePasswordValidator = changePasswordValidator;
        _resetPasswordValidator = resetPasswordValidator;
        _callerDetailProvider = callerDetailProvider;
        _passwordHasher = passwordHasher;
        _exceptionHandler = exceptionHandler;
    } 
    #endregion
    
    #region Methods
    public async Task VerifyUserNameAndPasswordAsync(VerifyUserNameAndPasswordRequestDto requestDto)
    {
        _verifyUserNameAndPasswordValidator.ValidateAndThrow(requestDto);
        
        User user = await _context.Users
            .Where(u => u.UserName == requestDto.UserName)
            .Where(u => u.DeletedDateTime == null)
            .SingleOrDefaultAsync()
            ?? throw new OperationException(
               new object[] { nameof(requestDto.UserName) },
               ErrorMessages.NotFound.ReplaceResourceName(DisplayNames.User)
            );
        
        bool isPasswordCorrect = _passwordHasher.VerifyPassword(requestDto.Password, user.PasswordHash);
        if (!isPasswordCorrect)
        {
            throw new OperationException(
                new object[] { nameof(requestDto.Password) },
                ErrorMessages.Incorrect.ReplacePropertyName(DisplayNames.Password)
            );
        }
    }

    public async Task ChangePasswordAsync(ChangePasswordRequestDto requestDto)
    {
        _changePasswordValidator.ValidateAndThrow(requestDto);

        int callerId = _callerDetailProvider.GetId();
        User user = await _context.Users
            .Where(u => u.Id == callerId && u.DeletedDateTime == null)
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
        
        if (!_passwordHasher.VerifyPassword(requestDto.CurrentPassword, user.PasswordHash))
        {
            throw new OperationException(
                new object[] { nameof(requestDto.CurrentPassword) },
                ErrorMessages.Incorrect.ReplacePropertyName(DisplayNames.CurrentPassword)
            );
        }
        
        string newPasswordHash = _passwordHasher.HashPassword(requestDto.NewPassword);
        user.PasswordHash = newPasswordHash;

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
    
    public async Task ResetPasswordAsync(int id, ResetPasswordRequestDto requestDto)
    {
        _resetPasswordValidator.ValidateAndThrow(requestDto);
        
        User user = await _context.Users
            .Where(u => u.Id == id && u.DeletedDateTime == null)
            .SingleOrDefaultAsync()
            ?? throw new NotFoundException();
        
        if (!_authorizationService.CanResetUserPassword(user))
        {
            throw new AuthorizationException();
        }
        
        string newPasswordHash = _passwordHasher.HashPassword(requestDto.NewPassword);
        user.PasswordHash = newPasswordHash;

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
}