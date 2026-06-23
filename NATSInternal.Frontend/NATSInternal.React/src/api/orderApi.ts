import { httpClient } from "./httpClient";

export type OrderApi = {
  getListAsync(requestDto?: OrderListRequestDto): Promise<OrderListResponseDto>;
  getDetailAsync(id: number): Promise<OrderDetailResponseDto>;
  createAsync(requestDto: OrderUpsertRequestDto): Promise<number>;
  updateAsync(id: number, requestDto: OrderUpsertRequestDto): Promise<void>;
  deleteAsync(id: number): Promise<void>;
  getRevenueAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto>;
  getCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto>;
  getConsultantCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto>;
  getRetailCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto>;
  getTreatmentCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto>;
  getStatsMonthYearSeriesAsync(): Promise<StatsMonthYearResponseDto[]>;
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
  },
  async getCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto> {
    return await httpClient.getAsync("/orders/count", requestDto);
  },
  async getRevenueAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto> {
    return await httpClient.getAsync("/orders/revenue", requestDto);
  },
  async getConsultantCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto> {
    return await httpClient.getAsync("/orders/consultant-count", requestDto);
  },
  async getRetailCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto> {
    return await httpClient.getAsync("/orders/retail-count", requestDto);
  },
  async getTreatmentCountAsync(requestDto: CountOverTimeRangeRequestDto): Promise<CountOverTimeRangeResponseDto> {
    return await httpClient.getAsync("/orders/treatment-count", requestDto);
  },
  async getStatsMonthYearSeriesAsync(): Promise<StatsMonthYearResponseDto[]> {
    if (statsMonthYearResponseDtos == null) {
      const responseDtos = await httpClient.getAsync<StatsMonthYearResponseDto[]>("/orders/stats-month-year-series");
      statsMonthYearResponseDtos = responseDtos;
    }

    return statsMonthYearResponseDtos;
  }
};

let statsMonthYearResponseDtos: StatsMonthYearResponseDto[] | null = null;
