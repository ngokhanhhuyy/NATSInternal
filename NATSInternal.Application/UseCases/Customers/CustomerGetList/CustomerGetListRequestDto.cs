using JetBrains.Annotations;
using MediatR;
using NATSInternal.Application.Extensions;

namespace NATSInternal.Application.UseCases.Customers;

[UsedImplicitly]
public class CustomerGetListRequestDto : ISortableAndPageableListRequestDto, IRequest<CustomerGetListResponseDto>
{
    #region Properties
    public bool? SortByAscending { get; set; }
    public string? SortByFieldName { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
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
        LastName,
        FirstName,
        Birthday,
        CreatedDateTime,
        DebtRemainingAmount
    }
    #endregion
}