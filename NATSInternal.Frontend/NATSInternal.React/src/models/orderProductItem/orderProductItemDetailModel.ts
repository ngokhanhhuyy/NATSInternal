import { createProductBasicModel } from "../shared/productBasicModel";
import { getDisplayAmountText } from "@/helpers";

declare global {
  type OrderProductItemDetailModel = {
    id: number;
    amountBeforeVatPerUnit: number;
    vatPercentagePerUnit: number;
    quantity: number;
    product: ProductBasicModel;
    displayAmountBeforeVatPerUnit: string;
    displayAmountAfterVat: string;
  };
}

function create(responseDto: OrderProductItemDetailResponseDto): OrderProductItemDetailModel {
  const vatAmountPerUnit = responseDto.amountBeforeVatPerUnit * (responseDto.vatPercentagePerUnit / 100);
  const amountAfterVat = (responseDto.amountBeforeVatPerUnit + vatAmountPerUnit) * responseDto.quantity;

  return {
    id: responseDto.id,
    amountBeforeVatPerUnit: responseDto.amountBeforeVatPerUnit,
    vatPercentagePerUnit: responseDto.vatPercentagePerUnit,
    quantity: responseDto.quantity,
    product: createProductBasicModel(responseDto.product),
    displayAmountBeforeVatPerUnit: getDisplayAmountText(responseDto.amountBeforeVatPerUnit, { excludeSuffix: true }),
    displayAmountAfterVat: getDisplayAmountText(amountAfterVat, { excludeSuffix: true })
  };
}

export { create as createOrderProductItemDetailModel };
