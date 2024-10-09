namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductEngageableListRequestDto : IFinancialEngageableListRequestDto
{
    int? ProductId { get; set; }
}