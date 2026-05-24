import { createCustomerBasicModel } from "./customerBasicModel";
import { getDisplayDateString, getDisplayAmountText, getPaymentDetailRoutePath } from "@/helpers";

declare global {
  type PaymentBasicModel = {
    id: number;
    type: PaymentType;
    statsDate: string;
    amount: number;
    customer: CustomerBasicModel;
    detailRoutePath: string;
    displayStatsDate: string;
    displayAmount: string;
  };
}

export function createPaymentBasicModel(responseDto: PaymentBasicResponseDto): PaymentBasicModel {
  return {
    id: responseDto.id,
    type: responseDto.type,
    statsDate: responseDto.statsDate,
    amount: responseDto.amount,
    customer: createCustomerBasicModel(responseDto.customer),
    detailRoutePath: getPaymentDetailRoutePath(responseDto.id),
    displayStatsDate: getDisplayDateString(responseDto.statsDate),
    displayAmount: getDisplayAmountText(responseDto.amount)
  };
}
