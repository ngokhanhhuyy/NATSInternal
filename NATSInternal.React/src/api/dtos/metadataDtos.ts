declare global {
  type DisplayNameListResponseDto = Record<string, string>;

  type ListOptionsResponseDto = {
    sortByFieldNameOptions: string[];
    defaultSortByFieldName: string | null;
    defaultSortByAscending: boolean | null;
    defaultResultsPerPage: number | null;
  };
  
  type ListOptionsListResponseDto = {
    user: ListOptionsResponseDto;
    customer: ListOptionsResponseDto;
    product: ListOptionsResponseDto;
  };

  type CreatingAuthorizationListResponseDto = {
    user: boolean;
    customer: boolean;
    product: boolean;
  };

  type MetadataResponseDto = {
    displayNameList: DisplayNameListResponseDto;
    listOptionsList: ListOptionsListResponseDto;
    creatingAuthorizationList: CreatingAuthorizationListResponseDto;
  };
}