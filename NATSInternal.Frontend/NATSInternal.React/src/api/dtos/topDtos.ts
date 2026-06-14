declare global {
  type TimeRangeUnitType = "Year" | "Month" | "Day";

  type TopRequestDto = Partial<{
    resultsCount: number;
    timeRangeUnitType: TimeRangeUnitType;
    timeRangeUnitCount: number;
  }>;

  type TopResponseDto<TBasicResponseDto, TMetric> = {
    item: TBasicResponseDto;
    metric: TMetric;
  };
}
