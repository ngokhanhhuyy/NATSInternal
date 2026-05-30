import { createCustomerBasicModel } from "../shared/customerBasicModel";
import { createCustomerUpsertModel } from "../customer/customerUpsertModel";
import { createOrderProductItemUpsertModel } from "../orderProductItem/orderProductItemUpsertModel";
import { createOrderServiceItemUpsertModel } from "../orderServiceItem/orderServiceItemUpsertModel";
import { getHTMLDateInputString, getCurrentDateHTMLInputString, getDateISOString } from "@/helpers";
import { getDisplayAmountText } from "@/helpers";

declare global {
  type OrderUpsertModel = {
    type: OrderType;
    statsDate: string;
    note: string;
    paidAmount: number;
    productItems: OrderProductItemUpsertModel[];
    serviceItems: OrderServiceItemUpsertModel[];
    photos: PhotoUpsertModel[];
    customer: OrderUpsertCustomerModel;
    toRequestDto(): OrderUpsertRequestDto;
    computeDisplayAmountAfterVat(): string;
  };

  type OrderUpsertCustomerModel = {
    basic: CustomerBasicModel | null;
    create: CustomerUpsertModel;
    createNewCustomer: boolean;
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
    customer: {
      basic: responseDto?.customer ? createCustomerBasicModel(responseDto.customer) : null,
      create: createCustomerUpsertModel(),
      createNewCustomer: !responseDto
    },
    toRequestDto(): OrderUpsertRequestDto {
      return {
        type: this.type,
        statsDate: this.statsDate ? getDateISOString(this.statsDate) : null,
        note: this.note || null,
        paidAmount: this.paidAmount,
        productItems: this.productItems.map(pi => pi.toRequestDto()),
        serviceItems: this.serviceItems.map(si => si.toRequestDto()),
        photos: this.photos.map(p => p.toRequestDto()),
        customer: {
          id: (!this.customer.createNewCustomer && this.customer.basic) ? this.customer.basic.id : null,
          create: this.customer.createNewCustomer && this.customer.create ? this.customer.create.toRequestDto() : null,
          createNewCustomer: this.customer.createNewCustomer
        }
      };
    },
    computeDisplayAmountAfterVat(): string {
      const productItemsAmountAfterVat = this.productItems.reduce((acc, productItem) => {
        const amountBeforeVatPerUnit = productItem.amountBeforeVatPerUnit;
        const vatAmountPerUnit = Math.ceil(amountBeforeVatPerUnit * (productItem.vatPercentagePerUnit / 100));
        const amountAfterVat = (amountBeforeVatPerUnit + vatAmountPerUnit) * productItem.quantity;
        return acc + amountAfterVat;
      }, 0);

      const servceItemsAmountAfterVat = this.serviceItems.reduce((acc, serviceItem) => {
        const amountBeforeVatPerUnit = serviceItem.amountBeforeVatPerUnit;
        const vatAmountPerUnit = Math.ceil(amountBeforeVatPerUnit * (serviceItem.vatPercentagePerUnit / 100));
        const amountAfterVat = (amountBeforeVatPerUnit + vatAmountPerUnit) * serviceItem.quantity;
        return acc + amountAfterVat;
      }, 0);

      return getDisplayAmountText(productItemsAmountAfterVat + servceItemsAmountAfterVat);
    }
  };
}
