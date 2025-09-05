using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Extensions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Exceptions;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

internal class UserDeleteHandler : IRequestHandler<UserDeleteRequestDto>
{
    #region Fields
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationService _authorizationService;
    private readonly IClock _clock;
    #endregion
    
    #region Constructors
    public UserDeleteHandler(
        IUserRepository repository,
        IUnitOfWork unitOfWork,
        IAuthorizationService authorizationService,
        IClock clock)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
        _clock = clock;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserDeleteRequestDto requestDto, CancellationToken cancellationToken)
    {
        // Fetch the user entity and ensure it exists in the database.
        User user = await _repository
            .GetUserByIdAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();
        
        // Ensure the requested user has permission to delete.
        if (!_authorizationService.CanDeleteUser(user))
        {
            throw new AuthorizationException();
        }
        
        // Mark user as deleted.
        try
        {
            user.MarkAsDeleted(_clock.Now);
            _repository.UpdateUser(user);
        }
        catch (DomainException)
        {
            throw new OperationException(
                new object[] { nameof(requestDto.Id) },
                ErrorMessages.AlreadyDeleted.ReplaceResourceName(DisplayNames.User)
            );
        }

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