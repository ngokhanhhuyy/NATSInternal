type OrderItem =
  | OrderProductItemDetailResponseDto
  | OrderProductItemDetailModel
  | OrderProductItemUpsertModel
  | OrderServiceItemDetailResponseDto
  | OrderServiceItemDetailModel
  | OrderServiceItemUpsertModel;

export function computeOrderItemAmountAfterVat(arg: OrderItem): number {
  const vatAmountPerUnit = arg.amountBeforeVatPerUnit * (arg.vatPercentagePerUnit / 100);
  const ceiledVatAmountPerUnit = Math.ceil(vatAmountPerUnit / 1000) * 1000;
  return (arg.amountBeforeVatPerUnit + ceiledVatAmountPerUnit) * arg.quantity;
}
