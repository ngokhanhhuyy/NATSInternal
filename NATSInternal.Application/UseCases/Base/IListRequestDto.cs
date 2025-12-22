namespace NATSInternal.Application.UseCases;

public interface IListRequestDto : IRequestDto
{
    #region Properties
    bool SortByAscending { get; set; }
    string SortByFieldName { get; set; }
    int Page { get; set; }
    int ResultsPerPage { get; set; }
    string? SearchContent { get; set; }
    #endregion
}