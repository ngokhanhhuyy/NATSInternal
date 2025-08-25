namespace NATSInternal.Core.Services;

/// <inheritdoc />
internal class AuthenticationService : IAuthenticationService
{
    #region Fields
    private readonly DatabaseContext _context;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IValidator<SignInRequestDto> _validator;
    #endregion

    #region Constructors
    public AuthenticationService(
            DatabaseContext context,
            IPasswordHashingService passwordHashingService,
            IValidator<SignInRequestDto> validator)
    {
        _context = context;
        _passwordHashingService = passwordHashingService;
        _validator = validator;
    }
    #endregion

    #region Methods
    /// <inheritdoc />
    public async Task<UserDetailResponseDto> VerifyUserNameAndPasswordAsync(
            SignInRequestDto requestDto,
            CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        // Fetch the password hash of the user.
        User user = await _context.Users
            .Include(u => u.Roles).ThenInclude(r => r.Permissions)
            .SingleOrDefaultAsync(u => u.UserName == requestDto.UserName, cancellationToken)
            ?? throw OperationException.NotFound(
                new object[] { nameof(requestDto.UserName) },
                DisplayNames.UserName);

        // Verify the password.
        bool isPasswordCorrect = _passwordHashingService.VerifyPassword(requestDto.Password, user.PasswordHash);
        if (!isPasswordCorrect)
        {
            throw new OperationException(
                new object[] { nameof(requestDto.Password) },
                ErrorMessages.Incorrect.ReplacePropertyName(DisplayNames.Password)
            );
        }

        return new UserDetailResponseDto(user);
    }
    #endregion
}
