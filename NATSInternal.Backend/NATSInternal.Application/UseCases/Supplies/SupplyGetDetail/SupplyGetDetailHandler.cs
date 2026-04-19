using JetBrains.Annotations;
using MediatR;
using NATSInternal.Application.Exceptions;
using NATSInternal.Domain.Features.Supplies;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Supplies;

[UsedImplicitly]
internal class SupplyGetDetailHandler : IRequestHandler<SupplyGetDetailRequestDto, SupplyGetDetailResponseDto>
{
    #region Fields
    private readonly ISupplyRepository _supplyRepository;
    private readonly IUserRepository _userRepository;
    #endregion
    
    #region Constructors
    public SupplyGetDetailHandler(ISupplyRepository supplyRepository, IUserRepository userRepository)
    {
        _supplyRepository = supplyRepository;
        _userRepository = userRepository;
    }
    #endregion
    
    #region Methods
    public async Task<SupplyGetDetailResponseDto> Handle(
        SupplyGetDetailRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        Supply supply = await _supplyRepository
            .GetSupplyByIdAsync(requestDto.Id, cancellationToken)
            ?? throw new NotFoundException();

        User? createdUser = await _userRepository.GetUserByIdAsync(supply.CreatedUserId, cancellationToken);
        if (!supply.LastUpdatedUserId.HasValue)
        {
            return new(supply, createdUser);
        }

        Guid lastUpdatedUserId = supply.LastUpdatedUserId.Value;
        User? lastUpdatedUser = await _userRepository.GetUserByIdAsync(lastUpdatedUserId, cancellationToken);
        return new(supply, createdUser, lastUpdatedUser);
    }
    #endregion
}