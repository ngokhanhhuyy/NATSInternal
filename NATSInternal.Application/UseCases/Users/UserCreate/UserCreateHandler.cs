using FluentValidation;
using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Exceptions;
using NATSInternal.Application.Localization;
using NATSInternal.Application.Security;
using NATSInternal.Application.Time;
using NATSInternal.Application.UnitOfWork;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

internal class UserCreateHandler : IRequestHandler<UserCreateRequestDto, Guid>
{
    #region Fields
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _repository;
    private readonly IValidator<UserCreateRequestDto> _validator;
    private readonly IAuthorizationService _authorizationService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IClock _clock;
    #endregion
    
    #region Constructors
    public UserCreateHandler(
        IUnitOfWork unitOfWork,
        IUserRepository repository,
        IValidator<UserCreateRequestDto> validator,
        IAuthorizationService authorizationService,
        IPasswordHasher passwordHasher,
        IClock clock)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _validator = validator;
        _passwordHasher = passwordHasher;
        _authorizationService = authorizationService;
        _clock = clock;
    }
    #endregion
    
    #region Methods
    public async Task<Guid> Handle(UserCreateRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        // Validate the data from the request.
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);
        
        // Ensure the requested user has permission to create.
        if (!_authorizationService.CanCreateUser())
        {
            throw new AuthorizationException();
        }

        // Hash the provided password and create the entity.
        string passwordHash = _passwordHasher.HashPassword(requestDto.Password);
        User user = new(requestDto.UserName, passwordHash, _clock.Now);
        _repository.AddUser(user);

        // Add the user to the requested roles.
        if (requestDto.RoleNames.Count > 0)
        {
            List<Role> roles = await _repository.GetRolesByNameAsync(requestDto.RoleNames, cancellationToken);
            for (int i = 0; i < requestDto.RoleNames.Count; i++)
            {
                Role role = roles
                    .SingleOrDefault(r => r.Name == requestDto.RoleNames[i])
                    ?? throw OperationException.NotFound(
                        new object[] { nameof(requestDto.RoleNames), i },
                        DisplayNames.Role
                    );
            }

            user.AddToRoles(roles);
        }

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.Id;
        }
        catch (PersistenceException exception)
        {
            if (exception.IsConcurrencyConflict || exception.IsForeignKeyConstraintViolation)
            {
                throw new ConcurrencyException();
            }

            if (exception.IsUniqueConstraintViolation)
            {
                throw OperationException.Duplicated(
                    new object[] { nameof(requestDto.UserName) },
                    DisplayNames.UserName
                );
            }

            throw;
        }
    }
    #endregion
}