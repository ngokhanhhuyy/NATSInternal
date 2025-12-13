declare global {
  type MetadataGetResponseDto = {
    displayNameList: MetadataGetDisplayNamesListResponseDto;
    listOptionsList: MetadataGetListOptionsListResponseDto;
    creatingAuthorizationList: MetadataGetCreatingAuthorizationListResponseDto;
  };

  type MetadataGetDisplayNamesListResponseDto = Record<string, string>;
  
  type MetadataGetListOptionsListResponseDto = {
    user: MetadataGetListOptionsResponseDto;
    customer: MetadataGetListOptionsResponseDto;
    product: MetadataGetListOptionsResponseDto;
    brand: MetadataGetListOptionsResponseDto;
  };

  type MetadataGetListOptionsResponseDto = {
    resourceName: string;
    sortByFieldNameOptions: string[];
    defaultSortByFieldName: string;
    defaultSortByAscending: boolean;
    defaultResultsPerPage: number;
  };

  type MetadataGetCreatingAuthorizationListResponseDto = {
    canCreateUser: boolean;
    canCreateCustomer: boolean;
    canCreateProduct: boolean;
    canCreateBrand: boolean;
    canCreateProductCategory: boolean;
  };
}