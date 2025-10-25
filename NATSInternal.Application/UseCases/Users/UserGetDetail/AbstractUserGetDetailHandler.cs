using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Exceptions;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Users;

internal abstract class AbstractUserGetDetailHandler<TRequestDto> : IRequestHandler<TRequestDto, UserGetDetailResponseDto>
    where TRequestDto : IRequestDto, IRequest<UserGetDetailResponseDto>
{
    #region Fields
    private readonly IUserRepository _repository;
    private readonly IAuthorizationInternalService _authorizationInternalService;
    #endregion

    #region Constructors
    protected AbstractUserGetDetailHandler(
        IUserRepository repository,
        IAuthorizationInternalService authorizationInternalService)
    {
        _repository = repository;
        _authorizationInternalService = authorizationInternalService;
    }
    #endregion

    #region AbstractMethods
    public abstract Task<UserGetDetailResponseDto> Handle(
        TRequestDto requestDto,
        CancellationToken cancellationToken = default);
    #endregion
    
    #region ProtectedMethods
    protected async Task<UserGetDetailResponseDto> Handle(
        Func<IUserRepository, CancellationToken, Task<User?>> userGetter,
        bool includingAuthorization = true,
        CancellationToken cancellationToken = default)
    {
        User user = await userGetter(_repository, cancellationToken) ?? throw new NotFoundException();
        if (!includingAuthorization)
        {
            return new(user);
        }

        UserExistingAuthorizationResponseDto authorizationResponseDto = _authorizationInternalService
            .GetUserExistingAuthorization(user);
        return new(user, authorizationResponseDto);
    }
    #endregion
}