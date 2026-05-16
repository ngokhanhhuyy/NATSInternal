import { httpClient } from "./httpClient";

export type ProductApi = {
  getListAsync(requestDto?: ProductListRequestDto): Promise<ProductListResponseDto>;
  getDetailAsync(id: number): Promise<ProductDetailResponseDto>;
  createAsync(requestDto: ProductCreateRequestDto): Promise<string>;
  updateAsync(id: number, requestDto: ProductUpdateRequestDto): Promise<void>;
  deleteAsync(id: number): Promise<void>;
};

export const productApi: ProductApi = {
  async getListAsync(requestDto?: ProductListRequestDto): Promise<ProductListResponseDto> {
    return await httpClient.getAsync("/products", requestDto);
  },
  async getDetailAsync(id: number): Promise<ProductDetailResponseDto> {
    return await httpClient.getAsync(`/products/${id}`);
  },
  async createAsync(requestDto: ProductCreateRequestDto): Promise<string> {
    return await httpClient.postAsync("/products",requestDto);
  },
  async updateAsync(id: number, requestDto: ProductUpdateRequestDto): Promise<void> {
    await httpClient.putAndIgnoreAsync(`/products/${id}`, requestDto);
  },
  async deleteAsync(id: number): Promise<void> {
    await httpClient.deleteAndIgnoreAsync(`/products/${id}`);
  }
};
