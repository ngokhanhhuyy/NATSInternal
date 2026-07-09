import { httpClient } from "./httpClient";

export type ProductApi = {
  getListAsync(requestDto?: ProductListRequestDto): Promise<ProductListResponseDto>;
  getAllAsync(): Promise<ProductMinimalResponseDto[]>;
  getDetailAsync(id: number): Promise<ProductDetailResponseDto>;
  createAsync(requestDto: ProductCreateRequestDto): Promise<number>;
  updateAsync(id: number, requestDto: ProductUpdateRequestDto): Promise<void>;
  deleteAsync(id: number): Promise<void>;
  getTopBySoldQuantity(requestDto: TopOverTimeRangeRequestDto):
    Promise<TopOverTimeRangeResponseDto<ProductBasicResponseDto, number>>;
  getTopByRevenue(requestDto: TopOverTimeRangeRequestDto):
    Promise<TopOverTimeRangeResponseDto<ProductBasicResponseDto, number>>;
};

export const productApi: ProductApi = {
  async getListAsync(requestDto?: ProductListRequestDto): Promise<ProductListResponseDto> {
    return await httpClient.getAsync("/products", requestDto);
  },
  async getAllAsync(): Promise<ProductMinimalResponseDto[]> {
    return await httpClient.getAsync("/products/all");
  },
  async getDetailAsync(id: number): Promise<ProductDetailResponseDto> {
    return await httpClient.getAsync(`/products/${id}`);
  },
  async createAsync(requestDto: ProductCreateRequestDto): Promise<number> {
    return await httpClient.postAsync("/products",requestDto);
  },
  async updateAsync(id: number, requestDto: ProductUpdateRequestDto): Promise<void> {
    await httpClient.putAndIgnoreAsync(`/products/${id}`, requestDto);
  },
  async deleteAsync(id: number): Promise<void> {
    await httpClient.deleteAndIgnoreAsync(`/products/${id}`);
  },
  async getTopBySoldQuantity(requestDto: TopOverTimeRangeRequestDto):
    Promise<TopOverTimeRangeResponseDto<ProductBasicResponseDto, number>>
  {
    return await httpClient.getAsync("/products/topBySoldQuantity", requestDto);
  },
  async getTopByRevenue(requestDto: TopOverTimeRangeRequestDto):
    Promise<TopOverTimeRangeResponseDto<ProductBasicResponseDto, number>>
  {
    return await httpClient.getAsync("/products/topByRevenue", requestDto);
  },
};
