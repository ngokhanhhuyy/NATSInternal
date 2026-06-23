declare global {
  type CountOverTimeRangeModel = {
    timeRangeUnitType: TimeRangeUnitType;
    timeRangeUnitCount: number;
    currentTimeRangeCount: number;
    previousTimeRangeCount: number;
    percentageComparedToPreviousTimeRange: number;
    toRequestDto(): CountOverTimeRangeRequestDto;
    mapFromResponseDto(responseDto: CountOverTimeRangeResponseDto): CountOverTimeRangeModel;
  };
}

export function createCountModel(
  responseDto?: CountOverTimeRangeResponseDto,
  requestDto?: CountOverTimeRangeRequestDto): CountOverTimeRangeModel
{
  const model: CountOverTimeRangeModel = {
    timeRangeUnitType: "Day",
    timeRangeUnitCount: 7,
    currentTimeRangeCount: responseDto?.currentTimeRangeCount ?? 0,
    previousTimeRangeCount: responseDto?.previousTimeRangeCount ?? 0,
    percentageComparedToPreviousTimeRange: responseDto?.percentageDiffComparedToPreviousTimeRange ?? 0,
    toRequestDto(): CountOverTimeRangeRequestDto {
      return {
        timeRangeUnitType: this.timeRangeUnitType,
        timeRangeUnitCount: this.timeRangeUnitCount
      };
    },
    mapFromResponseDto(responseDto: CountOverTimeRangeResponseDto): CountOverTimeRangeModel {
      return {
        ...this,
        currentTimeRangeCount: responseDto.currentTimeRangeCount,
        previousTimeRangeCount: responseDto.previousTimeRangeCount,
        percentageComparedToPreviousTimeRange: responseDto.percentageDiffComparedToPreviousTimeRange
      };
    }
  };

  if (requestDto) {
    model.timeRangeUnitType = requestDto.timeRangeUnitType ?? model.timeRangeUnitType;
    model.timeRangeUnitCount = requestDto.timeRangeUnitCount ?? model.timeRangeUnitCount;
  }

  return model;
}
