import { useHttpClient } from "./httpClient";

const httpClient = useHttpClient();

export type ProductCategoryApi = {
  getListAsync(requestDto?: ProductCategoryGetListRequestDto): Promise<ProductCategoryGetListResponseDto>;
  getAllAsync(): Promise<ProductCategoryBasicResponseDto[]>;
  getDetailAsync(id: string): Promise<ProductCategoryGetDetailResponseDto>;
  updateAsync(id: string, requestDto: ProductCategoryUpdateRequestDto): Promise<void>;
};

const productCategoryApi: ProductCategoryApi = {
  async getListAsync(requestDto?: ProductCategoryGetListRequestDto): Promise<ProductCategoryGetListResponseDto> {
    return await httpClient.getAsync("/products/categories", requestDto);
  },
  async getAllAsync(): Promise<ProductCategoryBasicResponseDto[]> {
    return await httpClient.getAsync("/products/categories/all");
  },
  async getDetailAsync(id: string): Promise<ProductCategoryGetDetailResponseDto> {
    return await httpClient.getAsync(`/products/categories/${id}`);
  },
  async updateAsync(id: string, requestDto: ProductCategoryUpdateRequestDto): Promise<void> {
    await httpClient.putAndIgnoreAsync(`/products/categories/${id}`, requestDto);
  }
};

export function useProductCategoryApi(): ProductCategoryApi {
  return productCategoryApi;
}