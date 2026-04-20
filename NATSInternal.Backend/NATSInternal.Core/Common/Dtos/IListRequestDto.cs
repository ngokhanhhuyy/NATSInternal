namespace NATSInternal.Core.Common.Dtos;

public interface IListRequestDto : IRequestDto
{
    #region Properties
    bool SortByAscending { get; set; }
    string SortByFieldName { get; set; }
    int Page { get; set; }
    int ResultsPerPage { get; set; }
    #endregion
}