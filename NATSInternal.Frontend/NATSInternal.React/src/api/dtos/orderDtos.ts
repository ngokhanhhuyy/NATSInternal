declare global {
  type OrderType = "Retail" | "Treatment" | "Consultant";

  type OrderListRequestDto = ImplementsPartial<IHasStatsListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    customerId: number;
    debtOrdersOnly: boolean;
    statsMonthYear: ListMonthYearRequestDto;
  }>;

  type OrderListResponseDto = Implements<IListResponseDto<OrderBasicResponseDto>, {
    items: OrderBasicResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type OrderDetailResponseDto = {
    id: number;
    statsDate: string;
    productItems: OrderProductItemDetailResponseDto[];
    serviceItems: OrderServiceItemDetailResponseDto[];
    note: string | null;
    createdDateTime: string;
    createdUser: UserBasicResponseDto;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicResponseDto;
    deletedDateTime: string | null;
    deletedUser: UserBasicResponseDto | null;
    authorization: OrderExistingAuthorizationResponseDto;
  };

  type OrderUpsertRequestDto = {
    type: OrderType;
    statsDate: string;
    note: string | null;
    paidAmount: number;
    productItems: OrderProductItemUpsertRequestDto[];
    serviceItems: OrderServiceItemUpsertRequestDto[];
    photos: PhotoUpsertRequestDto[];
  } & (
    {
      customerId: number;
      customer: null;
    } | {
      customerId: null;
      customer: CustomerUpsertRequestDto;
    }
  );
}
