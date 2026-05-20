import { createCustomerBasicModel } from "./customerBasicModel";

declare global {
  type OrderBasicModel = {
    id: number;
    type: OrderType;
    statsDate: string;
    amountAfterVat: number;
    customer: CustomerBasicModel;
    thumbnailUrl: string | null;
    authorization: OrderExistingAuthorizationResponseDto | null;
  };
}

export function createOrderBasicModel(responseDto: OrderBasicResponseDto): OrderBasicModel {
  return {
    id: responseDto.id,
    type: responseDto.type,
    statsDate: responseDto.statsDate,
    amountAfterVat: responseDto.amountAfterVat,
    customer: createCustomerBasicModel(responseDto.customer),
    thumbnailUrl: responseDto.thumbnailUrl,
    authorization: responseDto.authorization
  };
}
