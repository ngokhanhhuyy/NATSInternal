namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasProductItemRequestDto : IRequestDto
{
    #region Properties
    Guid? Id { get; set; }
    int Quantity { get; set; }
    Guid ProductId { get; set; }
    bool HasBeenChanged { get; set; }
    bool HasBeenDeleted { get; set; }
    #endregion
}