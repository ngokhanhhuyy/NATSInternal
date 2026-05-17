declare global {
  type OrderProductItemDetailResponseDto = {
    id: number;
    amountBeforeVatPerUnit: number;
    vatAmountPerUnit: number;
    quantity: number;
    product: ProductBasicResponseDto;
  };

  type OrderProductItemUpsertRequestDto = {
    id: number | null;
    amountBeforeVatPerUnit: number;
    vatAmountPerUnit: number;
    quantity: number;
    productId: number;
  };
}