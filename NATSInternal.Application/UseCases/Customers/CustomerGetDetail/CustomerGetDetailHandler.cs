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
    private readonly IUserRepository _userRepository;
    private readonly IAuthorizationInternalService _authorizationService;
    #endregion

    #region Constructors
    public CustomerGetDetailHandler(
        ICustomerRepository customerRepository,
        IUserRepository userRepository,
        IAuthorizationInternalService authorizationService)
    {
        _customerRepository = customerRepository;
        _userRepository = userRepository;
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

        User? createdUser = await _userRepository.GetUserByIdAsync(customer.CreatedUserId, cancellationToken);

        User? lastUpdatedUser = null;
        if (customer.LastUpdatedUserId.HasValue)
        {
            Guid lastUpdatedUserId = customer.LastUpdatedUserId.Value;
            lastUpdatedUser = await _userRepository.GetUserByIdAsync(lastUpdatedUserId, cancellationToken);
        }

        CustomerExistingAuthorizationResponseDto authorizationResponseDto = _authorizationService
            .GetCustomerExistingAuthorization(customer);

        return new CustomerGetDetailResponseDto(customer, createdUser, lastUpdatedUser, 0L, authorizationResponseDto);
    }
    #endregion
}
