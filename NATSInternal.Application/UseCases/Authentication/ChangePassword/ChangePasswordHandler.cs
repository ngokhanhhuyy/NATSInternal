using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Extensions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Security;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Authentication;

[UsedImplicitly]
internal class ChangePasswordHandler : IRequestHandler<ChangePasswordRequestDto>
{
    #region Fields
    private readonly IUserRepository _repository;
    private readonly IValidator<ChangePasswordRequestDto> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICallerDetailProvider _callerDetailProvider;
    private readonly IPasswordHasher _passwordHasher;
    #endregion
    
    #region Constructors
    public ChangePasswordHandler(
        IUserRepository repository,
        IValidator<ChangePasswordRequestDto> validator,
        IUnitOfWork unitOfWork,
        ICallerDetailProvider callerDetailProvider,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _callerDetailProvider = callerDetailProvider;
        _passwordHasher = passwordHasher;
    }
    #endregion
    
    #region Methods
    public async Task Handle(ChangePasswordRequestDto requestDto, CancellationToken cancellationToken)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        // Fetch the user entity.
        User user = await _repository
            .GetUserByIdAsync(_callerDetailProvider.GetId(), cancellationToken)
            ?? throw new NotFoundException();
        
        // Ensure the current password is correct.
        if (!_passwordHasher.VerifyPassword(requestDto.CurrentPassword, user.PasswordHash))
        {
            throw new OperationException(
                new object[] { nameof(requestDto.CurrentPassword) },
                ErrorMessages.Incorrect.ReplacePropertyName(DisplayNames.CurrentPassword)
            );
        }
        
        // Update the user's password hash.
        string passwordHash = _passwordHasher.HashPassword(requestDto.NewPassword);
        user.ChangePasswordHash(passwordHash);
        _repository.UpdateUser(user);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (PersistenceException exception)
        {
            if (exception.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }
    #endregion
}