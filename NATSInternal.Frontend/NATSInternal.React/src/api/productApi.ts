import { httpClient } from "./httpClient";

export type ProductApi = {
  getListAsync(requestDto?: ProductListRequestDto): Promise<ProductListResponseDto>;
  getDetailAsync(id: string): Promise<ProductDetailResponseDto>;
  createAsync(requestDto: ProductCreateRequestDto): Promise<string>;
  updateAsync(id: string, requestDto: ProductUpdateRequestDto): Promise<void>;
  deleteAsync(id: string): Promise<void>;
};

export const productApi: ProductApi = {
  async getListAsync(requestDto?: ProductListRequestDto): Promise<ProductListResponseDto> {
    return await httpClient.getAsync("/products", requestDto);
  },
  async getDetailAsync(id: string): Promise<ProductDetailResponseDto> {
    return await httpClient.getAsync(`/products/${id}`);
  },
  async createAsync(requestDto: ProductCreateRequestDto): Promise<string> {
    return await httpClient.postAsync("/products",requestDto);
  },
  async updateAsync(id: string, requestDto: ProductUpdateRequestDto): Promise<void> {
    await httpClient.putAndIgnoreAsync(`/products/${id}`, requestDto);
  },
  async deleteAsync(id: string): Promise<void> {
    await httpClient.deleteAndIgnoreAsync(`/products/${id}`);
  }
};
