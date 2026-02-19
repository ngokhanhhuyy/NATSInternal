import { useHttpClient } from "./httpClient";

export type CustomerApi = {
  getListAsync(requestDto?: CustomerGetListRequestDto): Promise<CustomerGetListResponseDto>;
  getDetailAsync(id: string): Promise<CustomerGetDetailResponseDto>;
  createAsync(requestDto: CustomerUpsertRequestDto): Promise<string>;
  updateAsync(id: string, requestDto: CustomerUpsertRequestDto): Promise<void>;
  deleteAsync(id: string): Promise<void>;
};

const httpClient = useHttpClient();

const customerApi: CustomerApi = {
  async getListAsync(requestDto?: CustomerGetListRequestDto): Promise<CustomerGetListResponseDto> {
    return httpClient.getAsync("/customers", requestDto);
  },
  async getDetailAsync(id: string): Promise<CustomerGetDetailResponseDto> {
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

export function useCustomerApi(): CustomerApi {
  return customerApi;
}