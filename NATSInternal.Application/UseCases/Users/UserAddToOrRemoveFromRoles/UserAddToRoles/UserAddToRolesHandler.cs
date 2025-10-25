using FluentValidation;
using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

internal class UserAddToRolesHandler : IRequestHandler<UserAddToRolesRequestDto>
{
    #region Fields
    private readonly IUserRepository _repository;
    private readonly IValidator<UserAddToRolesRequestDto> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthorizationInternalService _authorizationInternalService;
    #endregion
    
    #region Constructors
    public UserAddToRolesHandler(
        IUserRepository repository,
        IValidator<UserAddToRolesRequestDto> validator,
        IUnitOfWork unitOfWork,
        IAuthorizationInternalService authorizationInternalService)
    {
        _repository = repository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _authorizationInternalService = authorizationInternalService;
    }
    #endregion
    
    #region Methods
    public async Task Handle(UserAddToRolesRequestDto requestDto, CancellationToken cancellationToken)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        // Fetch the user entity.
        User user = await _repository
            .GetUserByIdAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        // Fetch the role entities based on the request.
        List<Role> existingRoles = await _repository.GetRolesByNameAsync(requestDto.RoleNames, cancellationToken);
        
        // Using dictionary here instead of existingRoles.SingleOrDefault() to avoid O(n) problem.
        Dictionary<string, Role> existingRolesDictionary = existingRoles.ToDictionary(r => r.Name);
        List<Guid> alreadyInRoleIds = user.Roles.Select(r => r.Id).ToList();

        // Add the user to each requested role.
        for (int i = 0; i < requestDto.RoleNames.Count; i++)
        {
            string roleName = requestDto.RoleNames[i];
            try
            {
                Role role = existingRolesDictionary[roleName];

                // Ensure the requested user has permission to add to roles.
                if (!_authorizationInternalService.CanAddUserToRole(user, role))
                {
                    throw new AuthorizationException();
                }

                if (alreadyInRoleIds.Contains(role.Id))
                {
                    throw new OperationException(
                        new object[] { nameof(requestDto.RoleNames), i },
                        ErrorMessages.UserAlreadyInRole.Replace("{RoleName}", roleName)
                    );
                }
            }
            catch (KeyNotFoundException)
            {
                throw OperationException.NotFound(
                    new object[] { nameof(requestDto.RoleNames), i },
                    DisplayNames.Role
                );
            }
        }

        user.AddToRoles(existingRoles);

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