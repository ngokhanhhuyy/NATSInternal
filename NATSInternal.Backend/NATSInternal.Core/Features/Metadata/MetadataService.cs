using JetBrains.Annotations;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Features.Authorization;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Expenses;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Core.Features.Payments;
using NATSInternal.Core.Features.Products;
using NATSInternal.Core.Features.Supplies;
using NATSInternal.Core.Features.Users;
using System.Reflection;

namespace NATSInternal.Core.Features.Metadata;

[UsedImplicitly]
internal class MetadataService : IMetadataService
{
    #region Fields
    private readonly IAuthorizationService _authorizationService;
    private readonly ISupplyService _supplyService;
    private readonly IOrderService _orderService;
    private static readonly IDictionary<string, string> _displayNames;
    #endregion
    
    #region Constructors
    static MetadataService()
    {
        FieldInfo[] fields = typeof(DisplayNames).GetFields(BindingFlags.Public | BindingFlags.Static);
        _displayNames = fields
            .Where(f => f.GetValue(null) is not null)
            .ToDictionary(f => f.Name, f => (string)f.GetValue(null)!);
    }
    
    public MetadataService(
        IAuthorizationService authorizationService,
        ISupplyService supplyService,
        IOrderService orderService)
    {
        _authorizationService = authorizationService;
        _supplyService = supplyService;
        _orderService = orderService;
    }
    #endregion
    
    #region Methods
    public async Task<MetadataResponseDto> GetMetadataAsync()
    {
        CustomerListRequestDto customerListRequestDto = new();
        ExpenseListRequestDto expenseListRequestDto = new();
        OrderListRequestDto orderListRequestDto = new();
        PaymentListRequestDto paymentListRequestDto = new();
        ProductListRequestDto productListRequestDto = new();
        SupplyListRequestDto supplyListRequestDto = new();
        UserListRequestDto userListRequestDto = new();

        MetadataListOptionsListResponseDto listOptionsList = new()
        {
            Customer = new()
            {
                ResourceName = nameof(Customer),
                DefaultSortByFieldName = customerListRequestDto.SortByFieldName,
                DefaultSortByAscending = customerListRequestDto.SortByAscending,
                DefaultResultsPerPage = customerListRequestDto.ResultsPerPage
            },
            Expense = new()
            {
                ResourceName = nameof(Expense),
                DefaultSortByFieldName = expenseListRequestDto.SortByFieldName,
                DefaultSortByAscending = expenseListRequestDto.SortByAscending,
                DefaultResultsPerPage = expenseListRequestDto.ResultsPerPage
            },
            Order = new()
            {
                ResourceName = nameof(Order),
                DefaultSortByFieldName = orderListRequestDto.SortByFieldName,
                DefaultSortByAscending = orderListRequestDto.SortByAscending,
                DefaultResultsPerPage = orderListRequestDto.ResultsPerPage
            },
            Payment = new()
            {
                ResourceName = nameof(Payment),
                DefaultSortByFieldName = paymentListRequestDto.SortByFieldName,
                DefaultSortByAscending = paymentListRequestDto.SortByAscending,
                DefaultResultsPerPage = paymentListRequestDto.ResultsPerPage
            },
            Product = new()
            {
                ResourceName = nameof(Product),
                DefaultSortByFieldName = productListRequestDto.SortByFieldName,
                DefaultSortByAscending = productListRequestDto.SortByAscending,
                DefaultResultsPerPage = productListRequestDto.ResultsPerPage
            },
            Supply = new()
            {
                ResourceName = nameof(Supply),
                DefaultSortByFieldName = supplyListRequestDto.SortByFieldName,
                DefaultSortByAscending = supplyListRequestDto.SortByAscending,
                DefaultResultsPerPage = supplyListRequestDto.ResultsPerPage
            },
            User = new()
            {
                ResourceName = nameof(User),
                DefaultSortByFieldName = userListRequestDto.SortByFieldName,
                DefaultSortByAscending = userListRequestDto.SortByAscending,
                DefaultResultsPerPage = userListRequestDto.ResultsPerPage
            }
        };
        
        return new()
        {
            DisplayNameList = _displayNames,
            ListOptionsList = listOptionsList,
            StatsMonthYearSeries = new()
            {
                SupplySeries = await _supplyService.GetStatsMonthYearSeriesAsync(),
                OrderSeries = await _orderService.GetStatsMonthYearSeriesAsync()
            },
            CreatingAuthorization = new()
            {
                CanCreateUser = _authorizationService.CanCreateUser(),
                CanCreateCustomer = _authorizationService.CanCreateCustomer(),
                CanCreateProduct = _authorizationService.CanCreateProduct(),
                CanCreateProductCategory = _authorizationService.CanCreateProductCategory(),
                CanCreateExpense = _authorizationService.CanCreateExpense(),
                CanCreateSupply = _authorizationService.CanCreateSupply(),
                CanCreateOrder = _authorizationService.CanCreateOrder(),
                CanCreatePayment = _authorizationService.CanCreatePayment()
            },
        };
    }
    #endregion
}
