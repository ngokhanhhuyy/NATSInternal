namespace NATSInternal.Application.UseCases;

public interface IPageableListRequestDto : IRequestDto
{
    #region Properties
    int Page { get; set; }
    int ResultsPerPage { get; set; }
    #endregion
}