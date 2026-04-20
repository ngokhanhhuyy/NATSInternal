namespace NATSInternal.Core.Common.Dtos;

public interface ISearchableListRequestDto : IListRequestDto
{
    #region Properties
    string? SearchContent { get; set; }
    #endregion
}