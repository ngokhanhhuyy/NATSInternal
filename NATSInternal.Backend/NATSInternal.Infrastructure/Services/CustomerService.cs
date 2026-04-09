using NATSInternal.Application.Authorization;
using NATSInternal.Application.Services;
using NATSInternal.Application.UseCases.Customers;
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
            string searchContent = requestDto.SearchContent.ToLower();
            query = query.Where(c =>
                c.FullName.ToLower().Contains(searchContent) ||
                c.NickName != null && c.NickName.ToLower().Contains(searchContent) ||
                c.PhoneNumber != null && c.PhoneNumber.Contains(searchContent) ||
                c.ZaloNumber != null && c.ZaloNumber.Contains(searchContent) ||
                c.FacebookUrl != null && c.FacebookUrl.ToLower().Contains(searchContent) ||
                c.Email != null && c.Email.ToLower().Contains(searchContent) ||
                c.Address != null && c.Address.ToLower().Contains(searchContent)
            );
        }

        if (requestDto.ExcludedIds.Count > 0)
        {
            query = query.Where(c => !requestDto.ExcludedIds.Contains(c.Id));
        }

        switch (requestDto.SortByFieldName)
        {
            case nameof(CustomerGetListRequestDto.FieldToSort.LastName):
                query = query.ApplySorting(p => p.LastName, requestDto.SortByAscending);
                break;
            case nameof(CustomerGetListRequestDto.FieldToSort.FirstName):
                query = query.ApplySorting(p => p.FirstName, requestDto.SortByAscending);
                break;
            case nameof(CustomerGetListRequestDto.FieldToSort.Birthday):
                query = query.ApplySorting(p => p.Birthday, requestDto.SortByAscending);
                break;
            case nameof(CustomerGetListRequestDto.FieldToSort.CreatedDateTime):
                query = query.ApplySorting(p => p.CreatedDateTime, requestDto.SortByAscending);
                break;
            case nameof(CustomerGetListRequestDto.FieldToSort.DebtRemainingAmount):
                break;
            default:
                throw new NotImplementedException();
        }

        Page<Customer> queryResult = await _listFetchingService.GetPagedListAsync(
            query,
            requestDto.Page,
            requestDto.ResultsPerPage,
            cancellationToken
        );

        IEnumerable<CustomerGetListCustomerResponseDto> productResponseDtos = queryResult.Items
            .Select(c => new CustomerGetListCustomerResponseDto(
                c,
                _authorizationInternalService.GetCustomerExistingAuthorization(c)));

        return new(productResponseDtos, queryResult.PageCount, queryResult.ItemCount);
    }
    #endregion
}