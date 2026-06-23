declare global {
  type CountOverTimeRangeRequestDto = ImplementsPartial<ITopAndCountOverTimeRangeRequestDto, {
    timeRangeUnitType: TimeRangeUnitType;
    timeRangeUnitCount: number;
  }>;

  type CountOverTimeRangeResponseDto = {
    currentTimeRangeCount: number;
    previousTimeRangeCount: number;
    percentageDiffComparedToPreviousTimeRange: number;
  };
}
