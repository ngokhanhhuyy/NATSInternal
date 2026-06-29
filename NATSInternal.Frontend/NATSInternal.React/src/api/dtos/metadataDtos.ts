declare global {
  type MetadataResponseDto = {
    displayNameList: MetadataDisplayNamesResponseDto;
    listOptionsList: MetadataListOptionsListResponseDto;
    statsMonthYearSeries: MetadataStatsMonthYearSeries;
    creatingAuthorization: MetadataCreatingAuthorizationResponseDto;
  };

  type MetadataDisplayNamesResponseDto = Record<string, string>;
  
  type MetadataListOptionsListResponseDto = {
    user: MetadataListOptionsResponseDto;
    customer: MetadataListOptionsResponseDto;
    product: MetadataListOptionsResponseDto;
    brand: MetadataListOptionsResponseDto;
    productCategory: MetadataListOptionsResponseDto;
    expense: MetadataListOptionsResponseDto;
    supply: MetadataListOptionsResponseDto;
    order: MetadataListOptionsResponseDto;
    payment: MetadataListOptionsResponseDto;
  };

  type MetadataListOptionsResponseDto = {
    resourceName: string;
    sortByFieldNameOptions: string[];
    defaultSortByFieldName: string;
    defaultSortByAscending: boolean;
    defaultResultsPerPage: number;
  };

  type MetadataStatsMonthYearSeries = {
    supplySeries: StatsMonthYearResponseDto[];
    orderSeries: StatsMonthYearResponseDto[];
  };

  type MetadataCreatingAuthorizationResponseDto = {
    canCreateUser: boolean;
    canCreateCustomer: boolean;
    canCreateProduct: boolean;
    canCreateBrand: boolean;
    canCreateProductCategory: boolean;
    canCreateExpense: boolean;
    canCreateSupply: boolean;
    canCreateOrder: boolean;
    canCreatePayment: boolean;
  };
}

export { };
