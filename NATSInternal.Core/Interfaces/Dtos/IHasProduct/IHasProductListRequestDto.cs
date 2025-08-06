namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasProductListRequestDto : IHasStatsListRequestDto
{
    int? ProductId { get; set; }
}