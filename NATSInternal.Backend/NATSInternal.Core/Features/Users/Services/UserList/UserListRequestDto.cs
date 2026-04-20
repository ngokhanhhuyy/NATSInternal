using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;

namespace NATSInternal.Core.Features.Users;

public class UserListRequestDto : ISearchableListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = true;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.UserName);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public string? SearchContent { get; set; }
    public List<int> RoleIds { get; set; } = new();
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
        UserName,
        RoleMaxPowerLevel
    }
    #endregion
}