import { useHttpClient } from "./httpClient";

const httpClient = useHttpClient();

export type BrandApi = {
  getListAsync(requestDto?: BrandGetListRequestDto): Promise<BrandGetListResponseDto>;
  getAllAsync(): Promise<BrandBasicResponseDto[]>;
  getDetailAsync(id: string): Promise<BrandGetDetailResponseDto>;
  createAsync(requestDto: BrandCreateRequestDto): Promise<string>;
  updateAsync(id: string, requestDto: BrandUpdateRequestDto): Promise<void>;
  deleteAsync(id: string): Promise<void>;
};

const brandApi: BrandApi = {
  async getListAsync(requestDto?: BrandGetListRequestDto): Promise<BrandGetListResponseDto> {
    return await httpClient.getAsync("/products/brands", requestDto);
  },

  async getAllAsync(): Promise<BrandBasicResponseDto[]> {
    return await httpClient.getAsync("/products/brands/all");
  },

  async getDetailAsync(id: string): Promise<BrandGetDetailResponseDto> {
    return await httpClient.getAsync(`/products/brands/${id}`);
  },

  async createAsync(requestDto: BrandCreateRequestDto): Promise<string> {
    return await httpClient.postAsync("/products/brands", requestDto);
  },

  async updateAsync(id: string, requestDto: BrandUpdateRequestDto): Promise<void> {
    return await httpClient.putAndIgnoreAsync(`/products/brands/${id}`, requestDto);
  },

  async deleteAsync(id: string): Promise<void> {
    return await httpClient.deleteAndIgnoreAsync(`/products/brands/${id}`);
  }
};

export function useBrandApi(): BrandApi {
  return brandApi;
}