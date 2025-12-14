using JetBrains.Annotations;
using MediatR;
using NATSInternal.Application.Extensions;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
public class ProductCategoryGetListRequestDto
    :
        IRequest<ProductCategoryGetListResponseDto>,
        ISortableListRequestDto,
        IPageableListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = true;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.Name);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public string? SearchContent { get; set; }
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
        Name,
        CreatedDateTime
    }
    #endregion
}