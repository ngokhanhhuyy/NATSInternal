namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasCustomerListRequestDto : ISortableAndPageableListRequestDto
{
    #region Properties
    Guid? CustomerId { get; set; }
    #endregion
}