namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasCustomerListRequestDto : ISortableListRequestDto
{
    #region Properties
    Guid? CustomerId { get; set; }
    #endregion
}