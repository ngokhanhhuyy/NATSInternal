using JetBrains.Annotations;
using MediatR;
using NATSInternal.Application.Extensions;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
public class ProductGetListRequestDto : ISortableAndPageableListRequestDto, IRequest<ProductGetListResponseDto>
{
    #region Properties
    public bool SortByAscending { get; set; } = true;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.Name);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public Guid? BrandId { get; set; }
    public Guid? CategoryId { get; set; }
    public string? SearchContent { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        BrandId = BrandId == Guid.Empty ? null : BrandId;
        CategoryId = CategoryId == Guid.Empty ? null : CategoryId;
        SearchContent = SearchContent.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        Name,
        DefaultAmountBeforeVatPerUnit,
        CreatedDateTime,
        StockingQuantity
    }
    #endregion
}