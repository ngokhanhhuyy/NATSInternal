using JetBrains.Annotations;
using MediatR;

namespace NATSInternal.Application.UseCases.Products;

[UsedImplicitly]
public class UserGetListRequestDto : ISortableAndPageableListRequestDto, IRequest<UserGetListResponseDto>
{
    #region Properties
    public bool? SortByAscending { get; set; }
    public string? SortByFieldName { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
    public string? SearchContent { get; set; }
    public Guid? RoleId { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        UserName,
        RoleMaxPowerLevel,
        CreatedDateTime
    }
    #endregion
}