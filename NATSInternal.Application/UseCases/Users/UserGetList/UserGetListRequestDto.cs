using JetBrains.Annotations;
using MediatR;

namespace NATSInternal.Application.UseCases.Users;

[UsedImplicitly]
public class UserGetListRequestDto : ISortableAndPageableListRequestDto, IRequest<UserGetListResponseDto>
{
    #region Properties
    public bool SortByAscending { get; set; } = true;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.RoleMaxPowerLevel);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public string SearchContent { get; set; } = string.Empty;
    public Guid? RoleId { get; set; }
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        CreatedDateTime,
        UserName,
        RoleMaxPowerLevel
    }
    #endregion
}