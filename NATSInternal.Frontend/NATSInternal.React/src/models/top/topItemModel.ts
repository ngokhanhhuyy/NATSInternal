declare global {
  type TopItemModel<TBasicModel extends object, TMetric> = {
    item: TBasicModel;
    metric: TMetric;
  };
}

export function createTopItemModel<TBasicResponseDto extends object, TBasicModel extends object, TMetric>(
  responseDto: TopItemResponseDto<TBasicResponseDto, TMetric>,
  createBasicModel: (basicResponseDto: TBasicResponseDto) => TBasicModel): TopItemModel<TBasicModel, TMetric>
{
  return {
    item: createBasicModel(responseDto.item),
    metric: responseDto.metric
  };
}
