using Microsoft.EntityFrameworkCore;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Services;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Customers;
using NATSInternal.Infrastructure.DbContext;
using NATSInternal.Infrastructure.Extensions;

namespace NATSInternal.Infrastructure.Services;

internal class CustomerService : ICustomerService
{
    #region Fields
    private readonly AppDbContext _context;
    private readonly IListFetchingService _listFetchingService;
    private readonly IAuthorizationInternalService _authorizationInternalService;
    #endregion
    
    #region Constructors
    public CustomerService(
        AppDbContext context,
        IListFetchingService listFetchingService,
        IAuthorizationInternalService authorizationInternalService)
    {
        _context = context;
        _listFetchingService = listFetchingService;
        _authorizationInternalService = authorizationInternalService;
    }
    #endregion
    
    #region Methods
    public async Task<CustomerGetListResponseDto> GetPaginatedCustomerListAsync(
        CustomerGetListRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Customer> query = _context.Customers;

        if (requestDto.SearchContent is not null && requestDto.SearchContent.Length > 0)
        {
            query = query.Where(c =>
                EF.Functions.Like(c.FullName.ToLower(), $"%{requestDto.SearchContent}%") ||
                EF.Functions.Like(c.PhoneNumber, $"%{requestDto.SearchContent}%") ||
                EF.Functions.Like(c.ZaloNumber, $"%{requestDto.SearchContent}%") ||
                EF.Functions.Like(c.FacebookUrl, $"%{requestDto.SearchContent}%") ||
                EF.Functions.Like(c.Email, $"%{requestDto.SearchContent}%") ||
                EF.Functions.Like(c.Address, $"%{requestDto.SearchContent}%") ||
                EF.Functions.Like(c.ZaloNumber, $"%{requestDto.SearchContent}%")
            );
        }

        bool sortByAscendingOrDefault = requestDto.SortByAscending ?? true;
        switch (requestDto.SortByFieldName)
        {
            case nameof(CustomerGetListRequestDto.FieldToSort.LastName) or null:
                query = query.ApplySorting(p => p.LastName, sortByAscendingOrDefault);
                break;
            case nameof(CustomerGetListRequestDto.FieldToSort.FirstName):
                query = query.ApplySorting(p => p.FirstName, sortByAscendingOrDefault);
                break;
            case nameof(CustomerGetListRequestDto.FieldToSort.Birthday):
                query = query.ApplySorting(p => p.Birthday, sortByAscendingOrDefault);
                break;
            case nameof(CustomerGetListRequestDto.FieldToSort.CreatedDateTime):
                query = query.ApplySorting(p => p.CreatedDateTime, sortByAscendingOrDefault);
                break;
            case nameof(CustomerGetListRequestDto.FieldToSort.DebtRemainingAmount):
                break;
            default:
                throw new NotImplementedException();
        }

        Page<Customer> productPage = await _listFetchingService.GetPagedListAsync(
            query,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        List<CustomerBasicResponseDto> productResponseDtos = productPage.Items
            .Select(customer => new CustomerBasicResponseDto(
                customer,
                0L,
                _authorizationInternalService.GetCustomerExistingAuthorization(customer)))
            .ToList();

        return new(productResponseDtos, productPage.PageCount);
    }
    #endregion
}