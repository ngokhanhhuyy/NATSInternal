using FluentValidation;
using MediatR;
using NATSInternal.Application.Services;

namespace NATSInternal.Application.UseCases.Users;

internal class UserGetListHandler : IRequestHandler<UserGetListRequestDto, UserGetListResponseDto>
{
    #region Fields
    private readonly IUserService _service;
    private readonly IValidator<UserGetListRequestDto> _validator;
    #endregion

    #region Constructors
    public UserGetListHandler(IUserService service, IValidator<UserGetListRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }
    #endregion

    #region Methods
    public async Task<UserGetListResponseDto> Handle(
        UserGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        return await _service.GetPaginatedUserListAsync(requestDto, cancellationToken);
    }
    #endregion
}