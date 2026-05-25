declare global {
  type OrderServiceItemUpsertModel = {
    id: number | null;
    name: string;
    amountBeforeVatPerUnit: number;
    vatPercentagePerUnit: number;
    quantity: number;
    toRequestDto(): OrderServiceItemUpsertRequestDto;
  };
}

function create(responseDto?: OrderServiceItemDetailResponseDto): OrderServiceItemUpsertModel {
  return {
    id: responseDto?.id ?? null,
    name: "",
    amountBeforeVatPerUnit: 0,
    vatPercentagePerUnit: 0,
    quantity: 1,
    toRequestDto(): OrderServiceItemUpsertRequestDto {
      return {
        id: this.id,
        name: this.name,
        amountBeforeVatPerUnit: this.amountBeforeVatPerUnit,
        vatPercentagePerUnit: this.vatPercentagePerUnit,
        quantity: this.quantity,
      };
    }
  };
}

export { create as createOrderServiceItemUpsertModel };
