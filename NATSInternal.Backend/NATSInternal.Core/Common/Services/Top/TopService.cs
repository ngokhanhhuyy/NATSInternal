using JetBrains.Annotations;
using FluentValidation;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Enums;
using NATSInternal.Core.Common.Time;

namespace NATSInternal.Core.Common.Services;

[UsedImplicitly]
internal class TopService : ITopService
{
    #region Fields
    private readonly IValidator<TopRequestDto> _validator;
    private readonly IClock _clock;
    #endregion

    #region Constructors
    public TopService(IValidator<TopRequestDto> validator, IClock clock)
    {
        _validator = validator;
        _clock = clock;
    }
    #endregion

    #region Methods
    public DateOnly ValidateAndGetEarliestDate(TopRequestDto requestDto)
    {
        _validator.ValidateAndThrow(requestDto);

        DateOnly today = _clock.Today;
        switch (requestDto.TimeRangeUnitType)
        {
            case TopTimeRangeUnitType.Year:
                return today.AddYears(-requestDto.TimeRangeUnitCount);
            case TopTimeRangeUnitType.Month:
                return today.AddMonths(-requestDto.TimeRangeUnitCount);
            default:
            case TopTimeRangeUnitType.Day:
                return today.AddDays(-requestDto.TimeRangeUnitCount);
        }
    }
    #endregion
}
