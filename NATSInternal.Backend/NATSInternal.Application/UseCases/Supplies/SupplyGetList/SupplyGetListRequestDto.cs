using JetBrains.Annotations;
using MediatR;
using NATSInternal.Application.Extensions;

namespace NATSInternal.Application.UseCases.Supplies;

[UsedImplicitly]
public class SupplyGetListRequestDto : IRequest<SupplyGetListResponseDto>, ITransactionListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = true;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.CreatedDateTime);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public string? SearchContent { get; set; } = string.Empty;
    #endregion

    #region Methods
    public void TransformValues()
    {
        SearchContent = SearchContent.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        CreatedDateTime,
        ItemAmount,
        TotalAmount
    }
    #endregion
}