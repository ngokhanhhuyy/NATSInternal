namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasProductListRequestDto : IHasStatsListRequestDto
{
    #region Properties
    int? ProductId { get; set; }
    #endregion
}