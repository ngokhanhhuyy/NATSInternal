declare global {
  type ProductCategoryGetListRequestDto = ImplementsPartial<
      ISearchableListRequestDto &
      IPageableListRequestDto &
      ISortableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
  }>;

  type ProductCategoryGetListResponseDto = Implements<
      IPageableListResponseDto<ProductCategoryGetListProductCategoryResponseDto>, {
    items: ProductCategoryGetListProductCategoryResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type ProductCategoryGetListProductCategoryResponseDto = Readonly<{
    id: string;
    name: string;
    countryName: string;
  }>;
}

export { };