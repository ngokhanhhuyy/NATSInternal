declare global {
  type CountRequestDto = ImplementsPartial<ITopAndCountRequestDto, {
    timeRangeUnitType: TimeRangeUnitType;
    timeRangeUnitCount: number;
  }>;

  type CountResponseDto = {
    currentTimeRangeCount: number;
    previousTimeRangeCount: number;
    percentageDiffComparedToPreviousTimeRange: number;
  };
}
