using MediatR;

namespace NATSInternal.Application.UseCases.Users;

public class UserListRequestDto : ISortableListRequestDto, IRequest<UserListResponseDto>
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
}