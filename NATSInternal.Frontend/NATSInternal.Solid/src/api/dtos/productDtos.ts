declare global {
  type ProductGetListRequestDto = ImplementsPartial<
      ISearchableListRequestDto &
      IPageableListRequestDto &
      ISortableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    brandId: string;
    categoryId: string;
    searchContent: string;
  }>;

  type AbstractProductUpsertRequestDto = {
    name: string;
    description: string | null;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    isForRetail: boolean;
    brandId: string | null;
    categoryName: string | null;
    photos: PhotoCreateOrUpdateRequestDto[];
  };
  
  type ProductCreateRequestDto = AbstractProductUpsertRequestDto;
  
  type ProductUpdateRequestDto = AbstractProductUpsertRequestDto & { id: string; isDiscontinued: boolean; };
  
  type ProductGetListProductResponseDto = {
    id: string;
    name: string;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    isResupplyNeeded: boolean;
    isDiscontinued: boolean;
    thumbnailUrl: string | null;
    category: ProductCategoryBasicResponseDto | null;
    brand: BrandBasicResponseDto | null;
    authorization: ProductExistingAuthorizationResponseDto;
  };

  type ProductGetListResponseDto = Implements<IPageableListResponseDto<ProductGetListProductResponseDto>, {
    items: ProductGetListProductResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type ProductGetDetailResponseDto = {
    id: string;
    name: string;
    description: string | null;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    isForRetail: boolean;
    isDiscontinued: boolean;
    createdDateTime: string;
    createdUser: UserBasicResponseDto;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicResponseDto | null;
    category: ProductCategoryBasicResponseDto | null;
    brand: BrandBasicResponseDto | null;
    stock: StockBasicResponseDto;
    photos: PhotoBasicResponseDto[];
    authorization: ProductExistingAuthorizationResponseDto;
  };
}