import { useHttpClient } from "./httpClient";

const httpClient = useHttpClient();

export type ProductCategoryApi = {
  getListAsync(requestDto?: ProductCategoryGetListRequestDto): Promise<ProductCategoryGetListResponseDto>;
  getAllAsync(): Promise<ProductCategoryBasicResponseDto[]>;
};

const productCategoryApi: ProductCategoryApi = {
  async getListAsync(requestDto?: ProductCategoryGetListRequestDto): Promise<ProductCategoryGetListResponseDto> {
    return await httpClient.getAsync("/products/categories", requestDto);
  },
  async getAllAsync(): Promise<ProductCategoryBasicResponseDto[]> {
    return await httpClient.getAsync("/products/categories/all");
  },
};

export function useProductCategoryApi(): ProductCategoryApi {
  return productCategoryApi;
}