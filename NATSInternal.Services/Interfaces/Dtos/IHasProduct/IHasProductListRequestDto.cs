namespace NATSInternal.Services.Interfaces.Dtos;

public interface IHasProductListRequestDto : IHasStatsListRequestDto
{
    int? ProductId { get; set; }
}