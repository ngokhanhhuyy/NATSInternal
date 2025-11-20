using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Exceptions;
using NATSInternal.Domain.Features.Customers;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Application.UseCases.Customers;

internal class CustomerGetDetailHandler : IRequestHandler<CustomerGetDetailRequestDto, CustomerGetDetailResponseDto>
{
    #region Fields
    private readonly ICustomerRepository _customerRepository;
    private readonly IAuthorizationInternalService _authorizationService;
    #endregion

    #region Constructors
    public CustomerGetDetailHandler(
        ICustomerRepository customerRepository,
        IUserRepository userRepository,
        IAuthorizationInternalService authorizationService)
    {
        _customerRepository = customerRepository;
        _authorizationService = authorizationService;
    }
    #endregion

    #region Methods
    public async Task<CustomerGetDetailResponseDto> Handle(
        CustomerGetDetailRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetCustomerByIdIncludingIntroducerAsync(
            requestDto.Id,
            cancellationToken
        );
        
        if (customer is null || customer.DeletedDateTime.HasValue)
        {
            throw new NotFoundException();
        }

        CustomerExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
            .GetCustomerExistingAuthorization(customer);

        return new(customer, authorizationResponseDto);
    }
    #endregion
}
