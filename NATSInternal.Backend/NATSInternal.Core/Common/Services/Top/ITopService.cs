using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Common.Services;

public interface ITopService
{
    #region Methods
    DateOnly ValidateAndGetEarliestDate(TopRequestDto requestDto);
    #endregion
}
