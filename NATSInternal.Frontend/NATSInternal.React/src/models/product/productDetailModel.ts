declare global {
  type ProductDetailModel = {
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
    photos: PhotoB[];
    authorization: ProductExistingAuthorizationResponseDto;
  };
}