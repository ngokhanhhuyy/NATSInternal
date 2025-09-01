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
    private readonly IAuthorizationService _authorizationService;
    #endregion

    #region Constructors
    protected AbstractUserGetDetailHandler(
        IUserRepository repository,
        IAuthorizationService authorizationService)
    {
        _repository = repository;
        _authorizationService = authorizationService;
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
        CancellationToken cancellationToken = default)
    {
        User user = await userGetter(_repository, cancellationToken) ?? throw new NotFoundException();
        UserExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
            .GetUserExistingAuthorization(user);

        return new(user, authorizationResponseDto);
    }
    #endregion
}