namespace NATSInternal.Core.Interfaces.Dtos;

internal interface IHasProductItemUpdateHistoryDataDto
{
    int Id { get; }
    long ProductAmountPerUnit { get; }
    int Quantity { get; }
    string ProductName { get; }
}