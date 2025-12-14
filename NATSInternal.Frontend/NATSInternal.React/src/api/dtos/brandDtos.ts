declare global {
  type BrandGetListRequestDto = ImplementsPartial<
      ISearchableListRequestDto &
      IPageableListRequestDto &
      ISortableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
  }>;

  type BrandGetListResponseDto = Implements<IPageableListResponseDto<BrandGetListBrandResponseDto>, {
    items: BrandGetListBrandResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type BrandGetListBrandResponseDto = {
    id: string;
    name: string;
    countryName: string;
  };
}