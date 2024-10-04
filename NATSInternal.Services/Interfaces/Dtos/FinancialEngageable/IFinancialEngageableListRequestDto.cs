namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableListRequestDto<TRequestDto>
    : IListRequestDto<TRequestDto>
{
    int Month { get; set; }
    int Year { get; set; }
}