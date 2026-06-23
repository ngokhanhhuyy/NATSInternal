declare global {
  type TopOverTimeRangeRequestDto = ImplementsPartial<ITopAndCountOverTimeRangeRequestDto, {
    resultsCount: number;
    timeRangeUnitType: TimeRangeUnitType;
    timeRangeUnitCount: number;
  }>;

  type TopOverTimeRangeResponseDto<TBasicResponseDto, TMetric> =
    TopOverTimeRangeItemResponseDto<TBasicResponseDto, TMetric>[];

  type TopOverTimeRangeItemResponseDto<TBasicResponseDto, TMetric> = {
    item: TBasicResponseDto;
    metric: TMetric;
  };
}
