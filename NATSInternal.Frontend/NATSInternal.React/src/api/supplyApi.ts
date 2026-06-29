import { httpClient } from "./httpClient";

export type SupplyApi = {
  getListAsync(requestDto?: SupplyListRequestDto): Promise<SupplyListResponseDto>;
  // getDetailAsync(id: number): Promise<OrderDetailResponseDto>;
  // createAsync(requestDto: OrderUpsertRequestDto): Promise<number>;
  // updateAsync(id: number, requestDto: OrderUpsertRequestDto): Promise<void>;
  // deleteAsync(id: number): Promise<void>;
  getStatsMonthYearSeriesAsync(): Promise<StatsMonthYearResponseDto[]>;
};

export const supplyApi: SupplyApi = {
  async getListAsync(requestDto?: SupplyListRequestDto): Promise<SupplyListResponseDto> {
    return await httpClient.getAsync("/supplies", requestDto);
  },
  // async getDetailAsync(id: number): Promise<OrderDetailResponseDto> {
  //   return await httpClient.getAsync(`/supplies/${id}`);
  // },
  // async createAsync(requestDto: OrderUpsertRequestDto): Promise<number> {
  //   return await httpClient.postAsync("/supplies",requestDto);
  // },
  // async updateAsync(id: number, requestDto: OrderUpsertRequestDto): Promise<void> {
  //   await httpClient.putAndIgnoreAsync(`/supplies/${id}`, requestDto);
  // },
  // async deleteAsync(id: number): Promise<void> {
  //   await httpClient.deleteAndIgnoreAsync(`/supplies/${id}`);
  // },
  async getStatsMonthYearSeriesAsync(): Promise<StatsMonthYearResponseDto[]> {
    if (statsMonthYearResponseDtos == null) {
      const responseDtos = await httpClient.getAsync<StatsMonthYearResponseDto[]>("/supplies/stats-month-year-series");
      statsMonthYearResponseDtos = responseDtos;
    }

    return statsMonthYearResponseDtos;
  }
};

let statsMonthYearResponseDtos: StatsMonthYearResponseDto[] | null = null;
