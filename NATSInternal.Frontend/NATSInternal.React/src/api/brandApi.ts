import { useHttpClient } from "./httpClient";

const httpClient = useHttpClient();

export type BrandApi = {
  getListAsync(requestDto?: BrandGetListRequestDto): Promise<BrandGetListResponseDto>;
  getAllAsync(): Promise<BrandBasicResponseDto[]>;
};

const brandApi: BrandApi = {
  async getListAsync(requestDto?: BrandGetListRequestDto): Promise<BrandGetListResponseDto> {
    return await httpClient.getAsync("/products/brands", requestDto);
  },

  async getAllAsync(): Promise<BrandBasicResponseDto[]> {
    return await httpClient.getAsync("/products/brands/all");
  }
};

export function useBrandApi(): BrandApi {
  return brandApi;
}