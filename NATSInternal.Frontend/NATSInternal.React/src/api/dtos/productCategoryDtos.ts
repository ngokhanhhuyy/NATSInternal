declare global {
  type ProductCategoryDetailResponseDto = {
    id: number;
    name: string;
    productCount: number;
    authorization: ProductCategoryExistingAuthorizationResponseDto | null;
  };

  type ProductCategoryUpsertRequestDto = {
    name: string;
  };
}

export { };
