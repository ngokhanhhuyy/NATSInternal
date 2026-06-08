import { getDisplayAmountText, computeOrderItemAmountAfterVat } from "@/helpers";

declare global {
  type OrderServiceItemDetailModel = {
    id: number;
    name: string;
    amountBeforeVatPerUnit: number;
    vatPercentagePerUnit: number;
    quantity: number;
    displayAmountBeforeVatPerUnit: string;
    displayAmountAfterVat: string;
  };
}

function create(responseDto: OrderServiceItemDetailResponseDto): OrderServiceItemDetailModel {
  return {
    id: responseDto.id,
    name: responseDto.name,
    amountBeforeVatPerUnit: responseDto.amountBeforeVatPerUnit,
    vatPercentagePerUnit: responseDto.vatPercentagePerUnit,
    quantity: responseDto.quantity,
    displayAmountBeforeVatPerUnit: getDisplayAmountText(responseDto.amountBeforeVatPerUnit),
    displayAmountAfterVat: getDisplayAmountText(computeOrderItemAmountAfterVat(responseDto))
  };
}

export { create as createOrderServiceItemDetailModel };
