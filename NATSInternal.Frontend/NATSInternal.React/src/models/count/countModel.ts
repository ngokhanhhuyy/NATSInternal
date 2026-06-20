declare global {
  type CountModel = {
    timeRangeUnitType: TimeRangeUnitType;
    timeRangeUnitCount: number;
    currentTimeRangeCount: number;
    previousTimeRangeCount: number;
    percentageComparedToPreviousTimeRange: number;
    toRequestDto(): CountRequestDto;
    mapFromResponseDto(responseDto: CountResponseDto): CountModel;
  };
}

export function createCountModel(responseDto?: CountResponseDto, requestDto?: CountRequestDto): CountModel {
  const model: CountModel = {
    timeRangeUnitType: "Day",
    timeRangeUnitCount: 7,
    currentTimeRangeCount: responseDto?.currentTimeRangeCount ?? 0,
    previousTimeRangeCount: responseDto?.previousTimeRangeCount ?? 0,
    percentageComparedToPreviousTimeRange: responseDto?.percentageDiffComparedToPreviousTimeRange ?? 0,
    toRequestDto(): CountRequestDto {
      return {
        timeRangeUnitType: this.timeRangeUnitType,
        timeRangeUnitCount: this.timeRangeUnitCount
      };
    },
    mapFromResponseDto(responseDto: CountResponseDto): CountModel {
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
