import { httpClient } from "./httpClient";

export type CustomerApi = {
  getListAsync(requestDto?: CustomerListRequestDto): Promise<CustomerListResponseDto>;
  getDetailAsync(id: string): Promise<CustomerDetailResponseDto>;
  createAsync(requestDto: CustomerUpsertRequestDto): Promise<string>;
  updateAsync(id: string, requestDto: CustomerUpsertRequestDto): Promise<void>;
  deleteAsync(id: string): Promise<void>;
};

export const customerApi: CustomerApi = {
  async getListAsync(requestDto?: CustomerListRequestDto): Promise<CustomerListResponseDto> {
    return httpClient.getAsync("/customers", requestDto);
  },
  async getDetailAsync(id: string): Promise<CustomerDetailResponseDto> {
    return httpClient.getAsync(`/customers/${id}`);
  },
  async createAsync(requestDto: CustomerUpsertRequestDto): Promise<string> {
    return httpClient.postAsync("/customers", requestDto);
  },
  async updateAsync(id: string, requestDto: CustomerUpsertRequestDto): Promise<void> {
    return httpClient.putAndIgnoreAsync(`/customers/${id}`, requestDto);
  },
  async deleteAsync(id: string): Promise<void> {
    return httpClient.deleteAndIgnoreAsync(`/customers/${id}`);
  }
};
