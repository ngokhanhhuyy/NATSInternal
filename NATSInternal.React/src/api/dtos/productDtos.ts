declare global {
  type ProductGetListRequestDto = Partial<{
    sortByAscending: string;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    brandId: string;
    categoryId: string;
    searchContent: string;
  }>;
  
  type ProductCreateRequestDto = AbstractProductUpsertRequestDto;
  
  type ProductUpdateRequestDto = AbstractProductUpsertRequestDto & { id: string; };
  
  type ProductGetListResponseDto = {
    items: ProductBasicResponseDto[];
    pageCount: number;
  };

  type ProductGetDetailResponseDto = {
    id: string;
    name: string;
    description: string | null;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    isForRetail: boolean;
    isDiscontinued: boolean;
    createdDateTime: string;
    lastUpdatedDateTime: string | null;
    createdUserId: string;
    lastUpdatedUserId: string | null;
    category: ProductCategoryBasicResponseDto | null;
    brand: BrandBasicResponseDto | null;
    photos: PhotoBasicResponseDto[];
    authorization: ProductExistingAuthorizationResponseDto;
  };
}

type AbstractProductUpsertRequestDto = {
  name: string;
  description: string | null;
  unit: string;
  defaultAmountBeforeVatPerUnit: number;
  defaultVatPercentagePerUnit: number;
  isForRetail: boolean;
  isDiscontinued: boolean;
  brandId: string | null;
  categoryName: string | null;
  photos: PhotoCreateOrUpdateRequestDto[];
};