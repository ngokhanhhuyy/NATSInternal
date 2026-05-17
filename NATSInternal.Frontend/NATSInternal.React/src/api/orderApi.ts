import { httpClient } from "./httpClient";

export type OrderApi = {
  getListAsync(requestDto?: OrderListRequestDto): Promise<OrderListResponseDto>;
  getDetailAsync(id: number): Promise<OrderDetailResponseDto>;
  createAsync(requestDto: OrderUpsertRequestDto): Promise<number>;
  updateAsync(id: number, requestDto: OrderUpsertRequestDto): Promise<void>;
  deleteAsync(id: number): Promise<void>;
};

export const orderApi: OrderApi = {
  async getListAsync(requestDto?: OrderListRequestDto): Promise<OrderListResponseDto> {
    return await httpClient.getAsync("/orders", requestDto);
  },
  async getDetailAsync(id: number): Promise<OrderDetailResponseDto> {
    return await httpClient.getAsync(`/orders/${id}`);
  },
  async createAsync(requestDto: OrderUpsertRequestDto): Promise<number> {
    return await httpClient.postAsync("/orders",requestDto);
  },
  async updateAsync(id: number, requestDto: OrderUpsertRequestDto): Promise<void> {
    await httpClient.putAndIgnoreAsync(`/orders/${id}`, requestDto);
  },
  async deleteAsync(id: number): Promise<void> {
    await httpClient.deleteAndIgnoreAsync(`/orders/${id}`);
  }
};