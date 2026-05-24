declare global {
  type OrderProductItemDetailResponseDto = {
    id: number;
    amountBeforeVatPerUnit: number;
    vatPercentagePerUnit: number;
    quantity: number;
    product: ProductBasicResponseDto;
  };

  type OrderProductItemUpsertRequestDto = {
    id: number | null;
    amountBeforeVatPerUnit: number;
    vatPercentagePerUnit: number;
    quantity: number;
    productId: number;
  };
}
