declare global {
  type TimeRangeUnitType = "Year" | "Month" | "Day";

  // RequestDtos.
  interface IListRequestDto {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
  }
  
  interface ISearchableListRequestDto extends IListRequestDto {
    searchContent: string | null;
  }
  
  interface IHasStatsListRequestDto extends IListRequestDto {
    statsYear: number | null;
    statsMonth: number | null;
  }
  
  interface IHasStatsUpsertRequestDto {
    statsDate: string;
    note: string | null;
  }
  
  interface IHasProductUpsertRequestDto<TItem extends IHasProductItemUpsertRequestDto>
    extends IHasStatsUpsertRequestDto
  {
    items: TItem[];
  }
  
  interface IHasProductItemUpsertRequestDto {
    id: number| null;
    productId: number;
    quantity: number;
  }
  
  // ResponseDtos.
  interface IListResponseDto<TBasic> {
    items: TBasic[];
    pageCount: number;
    itemCount: number;
  }

  interface ITopAndCountOverTimeRangeRequestDto {
    timeRangeUnitType: TimeRangeUnitType;
    timeRangeUnitCount: number;
  }
}

export { };
