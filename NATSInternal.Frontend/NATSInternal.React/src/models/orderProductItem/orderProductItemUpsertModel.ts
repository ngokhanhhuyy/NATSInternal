import { createProductBasicModel } from "../shared/productBasicModel";

declare global {
  type OrderProductItemUpsertModel = {
    id: number | null;
    amountBeforeVatPerUnit: number;
    vatPercentagePerUnit: number;
    quantity: number;
    product: ProductBasicModel;
    toRequestDto(): OrderProductItemUpsertRequestDto;
  };
}

function create(args: OrderProductItemDetailResponseDto | ProductBasicModel): OrderProductItemUpsertModel {
  if (Object.hasOwn(args, "defaultAmountBeforeVatPerUnit")) {
    const product = args as ProductBasicModel;
    return {
      id: null,
      amountBeforeVatPerUnit: product.defaultAmountBeforeVatPerUnit,
      vatPercentagePerUnit: product.defaultVatPercentagePerUnit,
      quantity: 1,
      product,
      toRequestDto(): OrderProductItemUpsertRequestDto {
        return convertToRequestDto(this);
      }
    };
  }

  const responseDto = args as OrderProductItemDetailResponseDto;
  return {
    id: responseDto.id,
    amountBeforeVatPerUnit: responseDto.amountBeforeVatPerUnit,
    vatPercentagePerUnit: responseDto.vatPercentagePerUnit,
    quantity: responseDto.quantity,
    product: createProductBasicModel(responseDto.product),
    toRequestDto(): OrderProductItemUpsertRequestDto {
      return convertToRequestDto(this);
    }
  };
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
