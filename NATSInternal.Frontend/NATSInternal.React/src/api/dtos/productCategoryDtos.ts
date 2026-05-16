declare global {
  type ProductCategoryDetailResponseDto = {
    id: number;
    name: string;
    authorization: ProductCategoryExistingAuthorizationResponseDto | null;
  };

  type ProductCategoryUpsertRequestDto = {
    name: string;
  };
}

export { };
