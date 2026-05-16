declare global {
  type MetadataResponseDto = {
    displayNameList: MetadataDisplayNamesResponseDto;
    listOptionsList: MetadataListOptionsListResponseDto;
    creatingAuthorization: MetadataCreatingAuthorizationResponseDto;
  };

  type MetadataDisplayNamesResponseDto = Record<string, string>;
  
  type MetadataListOptionsListResponseDto = {
    user: MetadataListOptionsResponseDto;
    customer: MetadataListOptionsResponseDto;
    product: MetadataListOptionsResponseDto;
    brand: MetadataListOptionsResponseDto;
    productCategory: MetadataListOptionsResponseDto;
  };

  type MetadataListOptionsResponseDto = {
    resourceName: string;
    sortByFieldNameOptions: string[];
    defaultSortByFieldName: string;
    defaultSortByAscending: boolean;
    defaultResultsPerPage: number;
  };

  type MetadataCreatingAuthorizationResponseDto = {
    canCreateUser: boolean;
    canCreateCustomer: boolean;
    canCreateProduct: boolean;
    canCreateBrand: boolean;
    canCreateProductCategory: boolean;
  };
}

export { };