import { httpClient } from "./httpClient";

export type CustomerApi = {
  getListAsync(requestDto?: CustomerListRequestDto): Promise<CustomerListResponseDto>;
  getDetailAsync(id: number): Promise<CustomerDetailResponseDto>;
  createAsync(requestDto: CustomerUpsertRequestDto): Promise<number>;
  updateAsync(id: number, requestDto: CustomerUpsertRequestDto): Promise<void>;
  deleteAsync(id: number): Promise<void>;
  getCountAsync(): Promise<number>;
  getHavingDebtAsync(): Promise<number>;
  getRefundNeededCountAsync(): Promise<number>;
  getNewCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto>;
  getPurchasedCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto>;
};

export const customerApi: CustomerApi = {
  async getListAsync(requestDto?: CustomerListRequestDto): Promise<CustomerListResponseDto> {
    return httpClient.getAsync("/customers", requestDto);
  },
  async getDetailAsync(id: number): Promise<CustomerDetailResponseDto> {
    return httpClient.getAsync(`/customers/${id}`);
  },
  async createAsync(requestDto: CustomerUpsertRequestDto): Promise<number> {
    return httpClient.postAsync("/customers", requestDto);
  },
  async updateAsync(id: number, requestDto: CustomerUpsertRequestDto): Promise<void> {
    return httpClient.putAndIgnoreAsync(`/customers/${id}`, requestDto);
  },
  async deleteAsync(id: number): Promise<void> {
    return httpClient.deleteAndIgnoreAsync(`/customers/${id}`);
  },
  async getCountAsync(): Promise<number> {
    return httpClient.getAsync("/customers/count");
  },
  async getHavingDebtAsync(): Promise<number> {
    return httpClient.getAsync("/customers/having-debt-count");
  },
  async getRefundNeededCountAsync(): Promise<number> {
    return httpClient.getAsync("/customers/refund-needed-count");
  },
  async getNewCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto> {
    return httpClient.getAsync("/customers/new-count", requestDto);
  },
  async getPurchasedCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto> {
    return httpClient.getAsync("/customers/purchased-count", requestDto);
  }
};
