declare global {
  type BrandGetListRequestDto = ImplementsPartial<ISortableAndPageableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
  }>;

  type BrandGetListResponseDto = Readonly<{
    items: BrandGetListBrandResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type BrandGetListBrandResponseDto = Readonly<{
    id: string;
    name: string;
    countryName: string;
  }>;
}