using FluentValidation;
using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Security;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

internal class UserResetPasswordHandler : IRequestHandler<UserResetPasswordRequestDto>
{
    #region Fields
    private readonly IUserRepository _repository;
    private readonly IValidator<UserResetPasswordRequestDto> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationService _authorizationService;
    private readonly IPasswordHasher _passwordHasher;
    #endregion
    
    #region Constructors
    public UserResetPasswordHandler(
        IUserRepository repository,
        IValidator<UserResetPasswordRequestDto> validator,
        IAuthorizationService authorizationService,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
        _passwordHasher = passwordHasher;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserResetPasswordRequestDto requestDto, CancellationToken cancellationToken)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        // Fetch the user entity.
        User user = await _repository
            .GetUserByIdAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();
        
        // Ensure the requesting user has permission to reset the target user's password.
        if (!_authorizationService.CanResetUserPassword(user))
        {
            throw new AuthorizationException();
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