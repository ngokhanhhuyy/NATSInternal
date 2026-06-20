declare global {
  type TopRequestDto = ImplementsPartial<ITopAndCountRequestDto, {
    resultsCount: number;
    timeRangeUnitType: TimeRangeUnitType;
    timeRangeUnitCount: number;
  }>;

  type TopResponseDto<TBasicResponseDto, TMetric> = TopItemResponseDto<TBasicResponseDto, TMetric>[];

  type TopItemResponseDto<TBasicResponseDto, TMetric> = {
    item: TBasicResponseDto;
    metric: TMetric;
  };
}
