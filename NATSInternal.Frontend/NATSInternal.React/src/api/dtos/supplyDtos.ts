declare global {
  type SupplyListRequestDto = ImplementsPartial<IHasStatsListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    productId: number;
    statsYear: number | null;
    statsMonth: number | null;
  }>;

  type SupplyListResponseDto = Implements<IListResponseDto<SupplyBasicResponseDto>, {
    items: SupplyBasicResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;
}