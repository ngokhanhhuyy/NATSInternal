using FluentValidation;
using MediatR;
using NATSInternal.Application.Services;

namespace NATSInternal.Application.UseCases.Customers;

internal class CustomerGetListHandler : IRequestHandler<CustomerGetListRequestDto, CustomerGetListResponseDto>
{
    #region Fields
    private readonly ICustomerService _service;
    private readonly IValidator<CustomerGetListRequestDto> _validator;
    #endregion

    #region Constructors
    public CustomerGetListHandler(
        ICustomerService service,
        IValidator<CustomerGetListRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }
    #endregion

    #region Methods
    public async Task<CustomerGetListResponseDto> Handle(
        CustomerGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        requestDto.TransformValues();
        _validator.ValidateAndThrow(requestDto);

        return await _service.GetPaginatedCustomerListAsync(requestDto, cancellationToken);
    }
    #endregion
}