import { createTopItemModel } from "./topOverTimeRangeItemModel";

declare global {
  type TopModel<TBasicResponseDto extends object, TBasicModel extends object, TMetric> = {
    resultsCount: number;
    timeRangeUnitType: TimeRangeUnitType;
    timeRangeUnitCount: number;
    items: TopItemModel<TBasicModel, TMetric>[];
    toRequestDto(): TopOverTimeRangeRequestDto;
    mapFromResponseDto(
      responseDto: TopOverTimeRangeResponseDto<TBasicResponseDto, TMetric>,
      basicModelMapper: (responseDto: TBasicResponseDto) => TBasicModel):
        TopModel<TBasicResponseDto, TBasicModel, TMetric>;
  };
}

export function createTopModel<TBasicResponseDto extends object, TBasicModel extends object, TMetric>(
  basicModelMapper: (responseDto: TBasicResponseDto) => TBasicModel,
  responseDto?: TopOverTimeRangeResponseDto<TBasicResponseDto, TMetric>,
  requestDto?: TopOverTimeRangeRequestDto): TopModel<TBasicResponseDto, TBasicModel, TMetric>
{
  let model: TopModel<TBasicResponseDto, TBasicModel, TMetric> = {
    resultsCount: 5,
    timeRangeUnitType: "Day",
    timeRangeUnitCount: 7,
    items: [],
    toRequestDto(): TopOverTimeRangeRequestDto {
      return {
        resultsCount: this.resultsCount,
        timeRangeUnitType: this.timeRangeUnitType,
        timeRangeUnitCount: this.timeRangeUnitCount
      };
    },
    mapFromResponseDto(
      responseDto: TopOverTimeRangeResponseDto<TBasicResponseDto, TMetric>,
      basicModelMapper: (responseDto: TBasicResponseDto) => TBasicModel):
        TopModel<TBasicResponseDto, TBasicModel, TMetric>
    {
      return { ...this, items: responseDto.map(dto => createTopItemModel(dto, basicModelMapper)) };
    }
  };

  if (responseDto) {
    model = model.mapFromResponseDto(responseDto, basicModelMapper);
  }

  if (requestDto) {
    model.resultsCount = requestDto.resultsCount ?? model.resultsCount;
    model.timeRangeUnitType = requestDto.timeRangeUnitType ?? model.timeRangeUnitType;
    model.timeRangeUnitCount = requestDto.timeRangeUnitCount ?? model.timeRangeUnitCount;
  }

  return model;
}
