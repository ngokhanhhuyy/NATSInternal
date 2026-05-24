import { getDisplayAmountText } from "@/helpers";

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
  const vatAmountPerUnit = responseDto.amountBeforeVatPerUnit * (responseDto.vatPercentagePerUnit / 100);
  const amountAfterVat = (responseDto.amountBeforeVatPerUnit + vatAmountPerUnit) * responseDto.quantity;
  
  return {
    id: responseDto.id,
    name: responseDto.name,
    amountBeforeVatPerUnit: responseDto.amountBeforeVatPerUnit,
    vatPercentagePerUnit: responseDto.vatPercentagePerUnit,
    quantity: responseDto.quantity,
    displayAmountBeforeVatPerUnit: getDisplayAmountText(responseDto.amountBeforeVatPerUnit, { excludeSuffix: true }),
    displayAmountAfterVat: getDisplayAmountText(amountAfterVat, { excludeSuffix: true })
  };
}

export { create as createOrderServiceItemDetailModel };
