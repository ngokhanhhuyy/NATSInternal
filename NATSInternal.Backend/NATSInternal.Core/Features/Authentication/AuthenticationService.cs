using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Exceptions;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Features.Users;
using NATSInternal.Core.Persistence.DbContext;

namespace NATSInternal.Core.Features.Authentication;

internal class AuthenticationService : IAuthenticationService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IValidator<VerifyUserNameAndPasswordRequestDto> _verifyUserNameAndPasswordValidator;
    private readonly IPasswordHasher _passwordHasher;
    #endregion
    
    #region Constructors
    public AuthenticationService(
        AppDbContext context,
        IValidator<VerifyUserNameAndPasswordRequestDto> verifyUserNameAndPasswordValidator,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _verifyUserNameAndPasswordValidator = verifyUserNameAndPasswordValidator;
        _passwordHasher = passwordHasher;
    }
    #endregion
    
    #region Methods
    public async Task VerifyUserNameAndPasswordAsync(VerifyUserNameAndPasswordRequestDto requestDto)
    {
        requestDto.TransformValues();
        _verifyUserNameAndPasswordValidator.ValidateAndThrow(requestDto);
        
        User user = await _context.Users
            .SingleOrDefaultAsync(u => u.UserName == requestDto.UserName)
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
    #endregion
}