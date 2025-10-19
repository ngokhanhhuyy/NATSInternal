using FluentValidation;
using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Extensions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Security;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Authentication;

internal class VerifyUserNameAndPasswordHandler : IRequestHandler<VerifyUserNameAndPasswordRequestDto>
{
    #region Fields
    private readonly IUserRepository _repository;
    private readonly IValidator<VerifyUserNameAndPasswordRequestDto> _validator;
    private readonly IPasswordHasher _passwordHasher;
    #endregion

    #region Constructors
    public VerifyUserNameAndPasswordHandler(
        IUserRepository repository,
        IValidator<VerifyUserNameAndPasswordRequestDto> validator,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _validator = validator;
        _passwordHasher = passwordHasher;
    }
    #endregion

    #region Methods
    public async Task Handle(
        VerifyUserNameAndPasswordRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        // Ensure the user having the given username exists.
        User user = await _repository
            .GetUserByUserNameAsync(requestDto.UserName, cancellationToken)
            ?? throw new OperationException(
                new object[] { nameof(requestDto.UserName) },
                ErrorMessages.NotFound.ReplaceResourceName(DisplayNames.User)
            );

        // Verify the given password.
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