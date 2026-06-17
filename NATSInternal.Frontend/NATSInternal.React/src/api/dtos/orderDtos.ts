declare global {
  type OrderType = "Retail" | "Treatment" | "Consultant";

  type OrderListRequestDto = ImplementsPartial<IHasStatsListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    type: OrderType;
    customerId: number;
    productId: number;
    debtOrdersOnly: boolean;
    statsYear: number | null;
    statsMonth: number | null;
  }>;

  type OrderListResponseDto = Implements<IListResponseDto<OrderBasicResponseDto>, {
    items: OrderBasicResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type OrderDetailResponseDto = {
    id: number;
    statsDate: string;
    type: OrderType;
    productItems: OrderProductItemDetailResponseDto[];
    serviceItems: OrderServiceItemDetailResponseDto[];
    note: string | null;
    customer: CustomerBasicResponseDto;
    payment: PaymentBasicResponseDto | null;
    createdDateTime: string;
    createdUser: UserBasicResponseDto;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicResponseDto;
    deletedDateTime: string | null;
    deletedUser: UserBasicResponseDto | null;
    photos: PhotoBasicResponseDto[];
    authorization: OrderExistingAuthorizationResponseDto;
    amountAfterVat: number;
    paidAmount: number;
    debtAmount: number;
  };

  type OrderUpsertRequestDto = {
    type: OrderType;
    statsDate: string | null;
    note: string | null;
    paidAmount: number;
    productItems: OrderProductItemUpsertRequestDto[];
    serviceItems: OrderServiceItemUpsertRequestDto[];
    photos: PhotoUpsertRequestDto[];
    customer: OrderUpsertCustomerRequestDto;
  };

  type OrderUpsertCustomerRequestDto = {
    id: number | null;
    create: CustomerUpsertRequestDto | null;
    createNewCustomer: boolean;
  };
}
