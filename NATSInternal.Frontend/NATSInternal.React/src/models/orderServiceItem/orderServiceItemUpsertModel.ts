import { getDisplayAmountText } from "@/helpers";

declare global {
  type OrderServiceItemUpsertModel = {
    id: number | null;
    name: string;
    amountBeforeVatPerUnit: number;
    vatPercentagePerUnit: number;
    quantity: number;
    toRequestDto(): OrderServiceItemUpsertRequestDto;
    readonly displayAmountBeforeVatPerUnit: string;
    readonly guid: string;
  };
}

function create(responseDto?: OrderServiceItemDetailResponseDto): OrderServiceItemUpsertModel {
  return {
    id: responseDto?.id ?? null,
    name: responseDto?.name ?? "",
    amountBeforeVatPerUnit: responseDto?.amountBeforeVatPerUnit ?? 0,
    vatPercentagePerUnit: responseDto?.vatPercentagePerUnit ?? 0,
    quantity: responseDto?.quantity ?? 1,
    toRequestDto(): OrderServiceItemUpsertRequestDto {
      return {
        id: this.id,
        name: this.name,
        amountBeforeVatPerUnit: this.amountBeforeVatPerUnit,
        vatPercentagePerUnit: this.vatPercentagePerUnit,
        quantity: this.quantity,
      };
    },
    get displayAmountBeforeVatPerUnit(): string {
      return getDisplayAmountText(this.amountBeforeVatPerUnit, { excludeSuffix: true });
    },
    guid: crypto.randomUUID()
  };
}

export { create as createOrderServiceItemUpsertModel };
