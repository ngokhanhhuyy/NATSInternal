declare global {
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
    statsMonthYear: ListMonthYearRequestDto | null;
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
}

export { };