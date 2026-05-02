namespace NATSInternal.Core.Common.Dtos;

public interface IHasProductItemUpsertRequestDto : IRequestDto
{
    #region Properties
    int? Id { get; set; }
    int ProductId { get; set; }
    int Quantity { get; set; }
    #endregion
}