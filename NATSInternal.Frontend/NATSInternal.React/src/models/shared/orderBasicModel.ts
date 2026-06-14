import { createCustomerBasicModel } from "./customerBasicModel";
import { getOrderDetailRoutePath, getDisplayDateString, getDisplayAmountText } from "@/helpers";
import { getDisplayName } from "@/metadata";

declare global {
  type OrderBasicModel = {
    id: number;
    type: OrderType;
    statsDate: string;
    amountAfterVat: number;
    debtAmount: number;
    customer: CustomerBasicModel;
    thumbnailUrl: string | null;
    authorization: OrderExistingAuthorizationResponseDto | null;
    detailRoutePath: string;
    displayName: string;
    displayStatsDate: string;
    displayAmountAfterVat: string;
  };
}

export function createOrderBasicModel(responseDto: OrderBasicResponseDto): OrderBasicModel {
  return {
    id: responseDto.id,
    type: responseDto.type,
    statsDate: responseDto.statsDate,
    amountAfterVat: responseDto.amountAfterVat,
    debtAmount: responseDto.debtAmount,
    customer: createCustomerBasicModel(responseDto.customer),
    thumbnailUrl: responseDto.thumbnailUrl,
    authorization: responseDto.authorization,
    displayName: `#${responseDto.id.toString()} ${getDisplayName(responseDto.type)}`,
    detailRoutePath: getOrderDetailRoutePath(responseDto.id),
    displayStatsDate: getDisplayDateString(responseDto.statsDate),
    displayAmountAfterVat: getDisplayAmountText(responseDto.amountAfterVat, { suffix: " vnđ" })
  };
}
