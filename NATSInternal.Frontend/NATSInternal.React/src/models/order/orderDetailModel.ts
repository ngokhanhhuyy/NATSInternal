import { createOrderProductItemDetailModel } from "../orderProductItem/orderProductItemDetailModel";
import { createOrderServiceItemDetailModel } from "../orderServiceItem/orderServiceItemDetailModel";
import { createCustomerBasicModel } from "../shared/customerBasicModel";
import { createPaymentBasicModel } from "../shared/paymentBasicModel";
import { createUserBasicModel } from "../shared/userBasicModel";
import { createPhotoBasicModel } from "../shared/photoBasicModel";
import { getDisplayName } from "@/metadata";
import { getDisplayAmountText, getDisplayDateString, getDisplayDateTimeString } from "@/helpers";
import { getOrderUpdateRoutePath } from "@/helpers";

declare global {
  type OrderDetailModel = {
    id: number;
    statsDate: string;
    type: OrderType;
    productItems: OrderProductItemDetailModel[];
    serviceItems: OrderServiceItemDetailModel[];
    note: string | null;
    customer: CustomerBasicModel;
    payment: PaymentBasicModel | null;
    createdDateTime: string;
    createdUser: UserBasicModel;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicModel | null;
    deletedDateTime: string | null;
    deletedUser: UserBasicModel | null;
    photos: PhotoBasicModel[];
    authorization: OrderExistingAuthorizationResponseDto;
    amountAfterVat: number;
    paidAmount: number;
    debtAmount: number;
    displayName: string;
    displayStatsDate: string;
    displayCreatedDateTime: string;
    displayLastUpdatedDateTime: string | null;
    displayDeletedDateTime: string | null;
    displayAmountAfterVat: string;
    displayPaidAmount: string;
    displayDebtAmount: string;
    updateRoutePath: string;
  };
}

export function createOrderDetailModel(responseDto: OrderDetailResponseDto): OrderDetailModel {
  return {
    id: responseDto.id,
    statsDate: responseDto.statsDate,
    type: responseDto.type,
    productItems: responseDto.productItems.map(createOrderProductItemDetailModel),
    serviceItems: responseDto.serviceItems.map(createOrderServiceItemDetailModel),
    note: responseDto.note,
    customer: createCustomerBasicModel(responseDto.customer),
    payment: responseDto.payment && createPaymentBasicModel(responseDto.payment),
    createdDateTime: responseDto.createdDateTime,
    createdUser: createUserBasicModel(responseDto.createdUser),
    lastUpdatedDateTime: responseDto.lastUpdatedDateTime,
    lastUpdatedUser: responseDto.lastUpdatedUser && createUserBasicModel(responseDto.lastUpdatedUser),
    deletedDateTime: responseDto.deletedDateTime,
    deletedUser: responseDto.deletedUser && createUserBasicModel(responseDto.deletedUser),
    photos: responseDto.photos.map(createPhotoBasicModel),
    authorization: responseDto.authorization,
    amountAfterVat: responseDto.amountAfterVat,
    paidAmount: responseDto.paidAmount,
    debtAmount: responseDto.debtAmount,
    displayName: `#${responseDto.id.toString()} ${getDisplayName(responseDto.type)}`,
    displayStatsDate: getDisplayDateString(responseDto.statsDate),
    displayCreatedDateTime: getDisplayDateTimeString(responseDto.createdDateTime),
    displayLastUpdatedDateTime: responseDto.lastUpdatedDateTime &&
      getDisplayDateTimeString(responseDto.lastUpdatedDateTime),
    displayDeletedDateTime: responseDto.deletedDateTime && getDisplayDateTimeString(responseDto.deletedDateTime),
    displayAmountAfterVat: getDisplayAmountText(responseDto.amountAfterVat),
    displayPaidAmount: getDisplayAmountText(responseDto.paidAmount),
    displayDebtAmount: getDisplayAmountText(responseDto.debtAmount),
    updateRoutePath: getOrderUpdateRoutePath(responseDto.id)
  };
}
