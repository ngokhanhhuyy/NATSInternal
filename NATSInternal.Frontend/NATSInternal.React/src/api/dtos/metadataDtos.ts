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
  };

  type MetadataGetListOptionsResponseDto = {
    resourceName: string;
    sortByFieldNameOptions: string[];
    defaultSortByFieldName: string | null;
    defaultSortByAscending: boolean | null;
    defaultResultsPerPage: number | null;
  };

  type MetadataGetCreatingAuthorizationListResponseDto = {
    canCreateUser: boolean;
    canCreateCustomer: boolean;
    canCreateProduct: boolean;
    canCreateBrand: boolean;
    canCreateProductCategory: boolean;
  };
}