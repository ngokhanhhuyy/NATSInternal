using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using NATSInternal.Application.Services;

namespace NATSInternal.Application.UseCases.Supplies;

[UsedImplicitly]
internal class SupplyGetListHandler : IRequestHandler<SupplyGetListRequestDto, SupplyGetListResponseDto>
{
    #region Fields
    private readonly ISupplyService _supplyService;
    private readonly IValidator<SupplyGetListRequestDto> _validator;
    #endregion
    
    #region Constructors
    public SupplyGetListHandler(ISupplyService supplyService, IValidator<SupplyGetListRequestDto> validator)
    {
        _supplyService = supplyService;
        _validator = validator;
    }
    #endregion
    
    #region Methods
    public async Task<SupplyGetListResponseDto> Handle(
        SupplyGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        return await _supplyService.GetPaginatedSupplyListAsync(requestDto, cancellationToken);
    }
    #endregion
}