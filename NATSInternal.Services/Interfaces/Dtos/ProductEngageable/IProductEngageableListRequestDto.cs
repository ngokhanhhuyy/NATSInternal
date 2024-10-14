namespace NATSInternal.Services.Interfaces.Dtos;

public interface IProductEngageableListRequestDto : IFinancialEngageableListRequestDto
{
    int? ProductId { get; set; }
}