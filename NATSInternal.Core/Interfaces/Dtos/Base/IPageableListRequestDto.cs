namespace NATSInternal.Core.Interfaces.Dtos;

public interface IPageableListRequestDto : IRequestDto
{
    #region Properties
    int Page { get; set; }
    int ResultsPerPage { get; set; }
    #endregion
}