import { useHttpClient } from "./httpClient";

export type ProductApi = {
  getListAsync(requestDto?: ProductGetListRequestDto): Promise<ProductGetListResponseDto>;
  getDetailAsync(id: string): Promise<ProductGetDetailResponseDto>;
  createAsync(requestDto: ProductCreateRequestDto): Promise<string>;
  updateAsync(id: string, requestDto: ProductUpdateRequestDto): Promise<void>;
  deleteAsync(id: string): Promise<void>;
};

const httpClient = useHttpClient();

const productApi: ProductApi = {
  async getListAsync(requestDto?: ProductGetListRequestDto): Promise<ProductGetListResponseDto> {
    return await httpClient.getAsync("/products", requestDto);
  },
  async getDetailAsync(id: string): Promise<ProductGetDetailResponseDto> {
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

export function useProductApi(): ProductApi {
  return productApi;
}