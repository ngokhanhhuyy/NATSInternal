using FluentValidation;
using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Exceptions;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

internal class UserRemoveFromRolesHandler : IRequestHandler<UserRemoveFromRolesRequestDto>
{
    #region Fields
    private readonly IUserRepository _repository;
    private readonly IValidator<UserRemoveFromRolesRequestDto> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationService _authorizationService;
    #endregion
    
    #region Constructors
    public UserRemoveFromRolesHandler(
        IUserRepository repository,
        IValidator<UserRemoveFromRolesRequestDto> validator,
        IUnitOfWork unitOfWork,
        IAuthorizationService authorizationService)
    {
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _authorizationService = authorizationService;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserRemoveFromRolesRequestDto requestDto, CancellationToken cancellationToken)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        // Fetch the user entity.
        User user = await _repository
            .GetUserByIdAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        // Using dictionary here instead of existingRoles.SingleOrDefault() to avoid O(n) problem.
        Dictionary<string, Role> addedRolesDictionary = user.Roles.ToDictionary(r => r.Name);

        for (int i = 0; i < requestDto.RoleNames.Count; i++)
        {
            string roleName = requestDto.RoleNames[i];
            try
            {
                Role role = addedRolesDictionary[roleName];
        
                // Ensure the requested user has permission to remove from roles.
                if (!_authorizationService.CanRemoveUserFromRole(user, role))
                {
                    throw new AuthorizationException();
                }
                
                user.RemoveFromRole(role);
            }
            catch (Exception exception)
            when (exception is KeyNotFoundException or DomainException)
            {
                throw new OperationException(
                    new object[] { nameof(requestDto.RoleNames), i },
                    ErrorMessages.UserNotInRole.Replace("{RoleName}", roleName)
                );
            }
        }

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (PersistenceException exception)
        {
            if (exception.IsForeignKeyConstraintViolation || exception.IsConcurrencyConflict)
            {
                throw new ConcurrencyException();
            }

            throw;
        }
    }
    #endregion
}