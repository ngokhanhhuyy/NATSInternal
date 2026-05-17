declare global {
  type OrderServiceItemDetailResponseDto = {
    id: number;
    name: string;
    amountBeforeVatPerUnit: number;
    vatAmountPerUnit: number;
    quantity: number;
  };

  type OrderServiceItemUpsertRequestDto = {
    id: number | null;
    name: string;
    amountBeforeVatPerUnit: number;
    vatAmountPerUnit: number;
    quantity: number;
  };
}