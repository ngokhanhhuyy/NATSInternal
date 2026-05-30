import { createProductBasicModel } from "../shared/productBasicModel";
import { getDisplayAmountText } from "@/helpers";

declare global {
  type OrderProductItemUpsertModel = {
    id: number | null;
    amountBeforeVatPerUnit: number;
    vatPercentagePerUnit: number;
    quantity: number;
    product: ProductBasicModel;
    toRequestDto(): OrderProductItemUpsertRequestDto;
    readonly displayAmountBeforeVatPerUnit: string;
    readonly guid: string;
  };
}

function create(args: OrderProductItemDetailResponseDto | ProductBasicModel): OrderProductItemUpsertModel {
  const model: OrderProductItemUpsertModel = {
    id: null,
    amountBeforeVatPerUnit: 0,
    vatPercentagePerUnit: 0,
    quantity: 1,
    product: null!,
    toRequestDto(): OrderProductItemUpsertRequestDto {
      return convertToRequestDto(this);
    },
    get displayAmountBeforeVatPerUnit(): string {
      return getDisplayAmountText(this.amountBeforeVatPerUnit, { excludeSuffix: true });
    },
    guid: crypto.randomUUID()
  };

  if (Object.hasOwn(args, "defaultAmountBeforeVatPerUnit")) {
    const product = args as ProductBasicModel;
    model.amountBeforeVatPerUnit = product.defaultAmountBeforeVatPerUnit;
    model.vatPercentagePerUnit = product.defaultVatPercentagePerUnit;
    model.product = product;
  } else {
    const responseDto = args as OrderProductItemDetailResponseDto;
    model.id = responseDto.id;
    model.amountBeforeVatPerUnit = responseDto.amountBeforeVatPerUnit;
    model.vatPercentagePerUnit = responseDto.vatPercentagePerUnit;
    model.quantity = responseDto.quantity;
    model.product = createProductBasicModel(responseDto.product);
  }

  return model;
}

function convertToRequestDto(model: OrderProductItemUpsertModel): OrderProductItemUpsertRequestDto {
  return {
    id: model.id,
    amountBeforeVatPerUnit: model.amountBeforeVatPerUnit,
    vatPercentagePerUnit: model.vatPercentagePerUnit,
    quantity: model.quantity,
    productId: model.product.id
  };
}

export { create as createOrderProductItemUpsertModel };
