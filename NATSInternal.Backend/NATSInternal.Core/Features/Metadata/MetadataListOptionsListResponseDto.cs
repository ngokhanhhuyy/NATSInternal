using CustomerListFieldToSort = NATSInternal.Core.Features.Customers.CustomerListRequestDto.FieldToSort;
using ExpenseListFieldToSort = NATSInternal.Core.Features.Expenses.ExpenseListRequestDto.FieldToSort;
using OrderListFieldToSort = NATSInternal.Core.Features.Orders.OrderListRequestDto.FieldToSort;
using PaymentListFieldToSort = NATSInternal.Core.Features.Payments.PaymentListRequestDto.FieldToSort;
using ProductListFieldToSort = NATSInternal.Core.Features.Products.ProductListRequestDto.FieldToSort;
using SupplyListFieldToSort = NATSInternal.Core.Features.Supplies.SupplyListRequestDto.FieldToSort;
using UserListFieldToSort = NATSInternal.Core.Features.Users.UserListRequestDto.FieldToSort;

namespace NATSInternal.Core.Features.Metadata;

public class MetadataListOptionsListResponseDto
{
    #region Properties
    public required MetadataListOptionsResponseDto<CustomerListFieldToSort> Customer { get; init; }
    public required MetadataListOptionsResponseDto<ExpenseListFieldToSort> Expense { get; init; }
    public required MetadataListOptionsResponseDto<OrderListFieldToSort> Order { get; init; }
    public required MetadataListOptionsResponseDto<PaymentListFieldToSort> Payment { get; init; }
    public required MetadataListOptionsResponseDto<ProductListFieldToSort> Product { get; init; }
    public required MetadataListOptionsResponseDto<SupplyListFieldToSort> Supply { get; init; }
    public required MetadataListOptionsResponseDto<UserListFieldToSort> User { get; init; }
    #endregion
}
