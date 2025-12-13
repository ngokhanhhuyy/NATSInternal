import { useHttpClient } from "./httpClient";

const httpClient = useHttpClient();

export type ProductCategoryApi = {
  getAllAsync(): Promise<ProductCategoryBasicResponseDto[]>;
};

const productCategoryApi: ProductCategoryApi = {
  async getAllAsync(): Promise<ProductCategoryBasicResponseDto[]> {
    return await httpClient.getAsync("/products/categories/all");
  },
};

export function useProductCategoryApi(): ProductCategoryApi {
  return productCategoryApi;
}