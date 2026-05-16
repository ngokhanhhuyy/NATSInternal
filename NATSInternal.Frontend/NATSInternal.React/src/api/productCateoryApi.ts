import { httpClient } from "./httpClient";

export type ProductCategoryApi = {
  getAllAsync(): Promise<ProductCategoryBasicResponseDto[]>;
  getDetailAsync(id: string): Promise<ProductCategoryDetailResponseDto>;
  createAsync(requestDto: ProductCategoryUpsertRequestDto): Promise<number>;
  updateAsync(id: string, requestDto: ProductCategoryUpsertRequestDto): Promise<void>;
};

export const productCategoryApi: ProductCategoryApi = {
  async getAllAsync(): Promise<ProductCategoryBasicResponseDto[]> {
    return await httpClient.getAsync("/products/categories/all");
  },
  async getDetailAsync(id: string): Promise<ProductCategoryDetailResponseDto> {
    return await httpClient.getAsync(`/products/categories/${id}`);
  },
  async createAsync(requestDto: ProductCategoryUpsertRequestDto): Promise<number> {
    return await httpClient.postAsync("/products/categories", requestDto);
  },
  async updateAsync(id: string, requestDto: ProductCategoryUpsertRequestDto): Promise<void> {
    await httpClient.putAndIgnoreAsync(`/products/categories/${id}`, requestDto);
  }
};
