declare global {
  type UserBasicResponseDto = {
    id: number;
    userName: string;
    roles: RoleBasicResponseDto[];
    isDeleted: boolean;
    authorization: UserExistingAuthorizationResponseDto | null;
  };

  type RoleBasicResponseDto = {
    id: number;
    name: string;
    displayName: string;
  };

  type CustomerBasicResponseDto = {
    id: number;
    fullName: string;
    nickName: string | null;
    debtAmount: number;
    isDeleted: boolean;
    authorization: CustomerExistingAuthorizationResponseDto | null;
  };
  
  type ProductBasicResponseDto = {
    id: number;
    name: string;
    unit: string;
    defaultAmountBeforeVatPerUnit: number;
    defaultVatPercentagePerUnit: number;
    stockingQuantity: number;
    isResupplyNeeded: boolean;
    isDiscontinued: boolean;
    thumbnailUrl: string | null;
    isDeleted: boolean;
    categories: ProductCategoryBasicResponseDto[];
    authorization: ProductExistingAuthorizationResponseDto | null;
  };
  
  type ProductCategoryBasicResponseDto = {
    id: number;
    name: string;
    productCount: number | null;
    authorization: ProductCategoryExistingAuthorizationResponseDto | null;
  };

  type OrderBasicResponseDto = {
    id: number;
    type: OrderType;
    statsDate: string;
    amountAfterVat: number;
    debtAmount: number;
    customer: CustomerBasicResponseDto;
    thumbnailUrl: string | null;
    authorization: OrderExistingAuthorizationResponseDto | null;
  };

  type PaymentBasicResponseDto = {
    id: number;
    type: PaymentType;
    statsDate: string;
    amount: number;
    customer: CustomerBasicResponseDto;
  };
  
  type PhotoBasicResponseDto = {
    id: number;
    url: string;
    isThumbnail: boolean;
  };

  type StatsMonthYearResponseDto = {
    year: number;
    month: number;
  };
}

export { };
