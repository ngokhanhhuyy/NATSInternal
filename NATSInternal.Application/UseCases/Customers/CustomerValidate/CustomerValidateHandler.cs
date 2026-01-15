using FluentValidation;
using MediatR;

namespace NATSInternal.Application.UseCases.Customers;

public class CustomerValidateHandler : IRequestHandler<CustomerValidateRequestDto>
{
    #region Fields
    private readonly IValidator<CustomerUpsertRequestDto> _validator;
    #endregion
    
    #region Constructors
    public CustomerValidateHandler(IValidator<CustomerUpsertRequestDto> validator)
    {
        _validator = validator;
    }
    #endregion
    
    #region Methods
    public async Task Handle(CustomerValidateRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        _validator.Validate(requestDto.Data);
        await Task.CompletedTask;
    }
    #endregion
}