namespace NATSInternal.Core.Common.Dtos;

public class ListMonthYearRequestDto : IRequestDto
{
    #region Properties
    public int Month { get; set; }
    public int Year { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
    }
    #endregion
}
