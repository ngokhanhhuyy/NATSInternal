import { createProductBasicModel } from "../shared/productBasicModel";
import { getDisplayAmountText, computeOrderItemAmountAfterVat } from "@/helpers";

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
  return {
    id: responseDto.id,
    amountBeforeVatPerUnit: responseDto.amountBeforeVatPerUnit,
    vatPercentagePerUnit: responseDto.vatPercentagePerUnit,
    quantity: responseDto.quantity,
    product: createProductBasicModel(responseDto.product),
    displayAmountBeforeVatPerUnit: getDisplayAmountText(responseDto.amountBeforeVatPerUnit),
    displayAmountAfterVat: getDisplayAmountText(computeOrderItemAmountAfterVat(responseDto))
  };
}

export { create as createOrderProductItemDetailModel };
