import { httpClient } from "./httpClient";

export type ProductCategoryApi = {
  getAllAsync(): Promise<ProductCategoryBasicResponseDto[]>;
  getDetailAsync(id: number): Promise<ProductCategoryDetailResponseDto>;
  createAsync(requestDto: ProductCategoryUpsertRequestDto): Promise<number>;
  updateAsync(id: number, requestDto: ProductCategoryUpsertRequestDto): Promise<void>;
  deleteAsync(id: number): Promise<void>;
};

export const productCategoryApi: ProductCategoryApi = {
  async getAllAsync(): Promise<ProductCategoryBasicResponseDto[]> {
    return await httpClient.getAsync("/products/categories");
  },
  async getDetailAsync(id: number): Promise<ProductCategoryDetailResponseDto> {
    return await httpClient.getAsync(`/products/categories/${id}`);
  },
  async createAsync(requestDto: ProductCategoryUpsertRequestDto): Promise<number> {
    return await httpClient.postAsync("/products/categories", requestDto);
  },
  async updateAsync(id: number, requestDto: ProductCategoryUpsertRequestDto): Promise<void> {
    await httpClient.putAndIgnoreAsync(`/products/categories/${id}`, requestDto);
  },
  async deleteAsync(id: number): Promise<void> {
    await httpClient.deleteAndIgnoreAsync(`/products/categories/${id}`);
  }
};
