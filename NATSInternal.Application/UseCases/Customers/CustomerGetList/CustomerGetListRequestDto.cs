using JetBrains.Annotations;
using MediatR;
using NATSInternal.Application.Extensions;

namespace NATSInternal.Application.UseCases.Customers;

[UsedImplicitly]
public class CustomerGetListRequestDto : IRequest<CustomerGetListResponseDto>, IListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = true;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.LastName);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 30;
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
        LastName,
        FirstName,
        Birthday,
        CreatedDateTime,
        DebtRemainingAmount
    }
    #endregion
}