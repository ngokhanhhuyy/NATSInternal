declare global {
  type OrderServiceItemDetailResponseDto = {
    id: number;
    name: string;
    amountBeforeVatPerUnit: number;
    vatPercentagePerUnit: number;
    quantity: number;
  };

  type OrderServiceItemUpsertRequestDto = {
    id: number | null;
    name: string;
    amountBeforeVatPerUnit: number;
    vatPercentagePerUnit: number;
    quantity: number;
  };
}
