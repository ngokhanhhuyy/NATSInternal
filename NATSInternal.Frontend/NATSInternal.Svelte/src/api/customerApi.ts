import { useHttpClient } from "./httpClient";

export type CustomerApi = {
  getListAsync(requestDto?: CustomerGetListRequestDto): Promise<CustomerGetListResponseDto>;
  getDetailAsync(id: string): Promise<CustomerGetDetailResponseDto>;
};

const httpClient = useHttpClient();

const customerApi: CustomerApi = {
  async getListAsync(requestDto?: CustomerGetListRequestDto): Promise<CustomerGetListResponseDto> {
    return httpClient.getAsync("/customers", requestDto);
  },
  async getDetailAsync(id: string): Promise<CustomerGetDetailResponseDto> {
    return httpClient.getAsync(`/customers/${id}`);
  }
};

export function useCustomerApi(): CustomerApi {
  return customerApi;
}