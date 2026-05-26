import { createCustomerBasicModel } from "../shared/customerBasicModel";
import { createCustomerUpsertModel } from "../customer/customerUpsertModel";
import { createOrderProductItemUpsertModel } from "../orderProductItem/orderProductItemUpsertModel";
import { createOrderServiceItemUpsertModel } from "../orderServiceItem/orderServiceItemUpsertModel";
import { getHTMLDateInputString, getCurrentDateHTMLInputString, getDateISOString } from "@/helpers";

declare global {
  type OrderUpsertModel = {
    type: OrderType;
    statsDate: string;
    note: string;
    paidAmount: number;
    productItems: OrderProductItemUpsertModel[];
    serviceItems: OrderServiceItemUpsertModel[];
    photos: PhotoUpsertModel[];
    customer: CustomerBasicModel | null;
    customerUpsert: CustomerUpsertModel;
    toRequestDto(): OrderUpsertRequestDto;
  };
}

export function createOrderUpsertModel(responseDto?: OrderDetailResponseDto): OrderUpsertModel {
  return {
    type: responseDto?.type ?? "Retail",
    statsDate: responseDto?.statsDate
      ? getHTMLDateInputString(responseDto.statsDate)
      : getCurrentDateHTMLInputString(),
    note: responseDto?.note ?? "",
    paidAmount: responseDto?.payment?.amount ?? 0,
    productItems: responseDto?.productItems.map(createOrderProductItemUpsertModel) ?? [],
    serviceItems: responseDto?.serviceItems.map(createOrderServiceItemUpsertModel) ?? [],
    photos: [],
    customer: responseDto?.customer ? createCustomerBasicModel(responseDto.customer) : null,
    customerUpsert: createCustomerUpsertModel(),
    toRequestDto(): OrderUpsertRequestDto {
      let customerProperties: OrderUpsertCustomerRequestDto;
      if (this.customer) {
        customerProperties = {
          customerId: this.customer.id,
          customer: null,
        };
      } else {
        customerProperties = {
          customerId: null,
          customer: this.customerUpsert.toRequestDto(),
        };
      }

      return {
        type: this.type,
        statsDate: this.statsDate ? getDateISOString(this.statsDate) : null,
        note: this.note || null,
        paidAmount: this.paidAmount,
        productItems: this.productItems.map(pi => pi.toRequestDto()),
        serviceItems: this.serviceItems.map(si => si.toRequestDto()),
        photos: this.photos.map(p => p.toRequestDto()),
        ...customerProperties
      };
    }
  };
}
