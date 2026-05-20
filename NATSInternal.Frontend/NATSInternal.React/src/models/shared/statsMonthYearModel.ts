declare global {
  type StatsMonthYearModel = {
    month: number;
    year: number;
    toRequestDto(): [number, number];
  };
}

export function createStatsMonthYearModel(responseDto: StatsMonthYearResponseDto): StatsMonthYearModel {
  return {
    year: responseDto.year,
    month: responseDto.month,
    toRequestDto(): [number, number] {
      return [this.year, this.month];
    }
  };
}
