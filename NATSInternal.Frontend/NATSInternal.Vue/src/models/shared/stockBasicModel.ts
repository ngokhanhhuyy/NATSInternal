declare global {
  type StockBasicModel = {
    id: string;
    stockingQuantity: number;
    resupplyThresholdQuantity: number;
  };
}

export function createStockBasicModel(responseDto: StockBasicResponseDto): StockBasicModel {
  return {
    id: responseDto.id,
    stockingQuantity: responseDto.stockingQuantity,
    resupplyThresholdQuantity: responseDto.resupplyThresholdQuantity
  };
}