declare global {
  type ProductListRequestDto = ImplementsPartial<ISearchableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    categoryId: number;
    searchContent: string | null;
  }>;

  type AbstractProductUpsertRequestDto = {
    name: string;
    description: string | null;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    resupplyThresholdQuantity: number | null;
    isForRetail: boolean;
    categoryIds: number[];
    photos: PhotoUpsertRequestDto[];
  };
  
  type ProductCreateRequestDto = AbstractProductUpsertRequestDto;
  
  type ProductUpdateRequestDto = AbstractProductUpsertRequestDto & { isDiscontinued: boolean; };

  type ProductListResponseDto = Implements<IListResponseDto<ProductBasicResponseDto>, {
    items: ProductBasicResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type ProductDetailResponseDto = {
    id: number;
    name: string;
    description: string | null;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    resupplyThresholdQuantity: number;
    isForRetail: boolean;
    isDiscontinued: boolean;
    createdDateTime: string;
    createdUser: UserBasicResponseDto;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicResponseDto | null;
    deletedDateTime: string | null;
    deletedUser: UserBasicResponseDto | null;
    categories: ProductCategoryBasicResponseDto[];
    photos: PhotoBasicResponseDto[];
    authorization: ProductExistingAuthorizationResponseDto;
  };
}
