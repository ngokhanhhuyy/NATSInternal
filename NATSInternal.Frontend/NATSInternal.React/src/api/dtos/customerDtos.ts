declare global {
  type Gender = "Male" | "Female";

  type CustomerListRequestDto = ImplementsPartial<ISearchableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    excludedIds: number[];
  }>;

  type CustomerListResponseDto = Implements<IListResponseDto<CustomerBasicResponseDto>, {
    items: CustomerBasicResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type CustomerDetailResponseDto = {
    id: number;
    firstName: string;
    middleName: string | null;
    lastName: string;
    fullName: string;
    nickName: string;
    gender: Gender;
    birthday: string | null;
    phoneNumber: string | null;
    zaloNumber: string | null;
    facebookUrl: string | null;
    email: string | null;
    address: string | null;
    note: string | null;
    createdDateTime: string;
    createdUser: UserBasicResponseDto;
    lastUpdatedDateTime: string | null;
    lastUpdatedUser: UserBasicResponseDto | null;
    deletedDateTime: string | null;
    deletedUser: UserBasicResponseDto | null;
    debtRemainingAmount: number;
    introducer: CustomerBasicResponseDto | null;
    authorization: CustomerExistingAuthorizationResponseDto;
  };

  type CustomerUpsertRequestDto = {
    firstName: string;
    middleName: string | null;
    lastName: string;
    nickName: string | null;
    gender: Gender;
    birthday: string | null;
    phoneNumber: string | null;
    zaloNumber: string | null;
    facebookUrl: string | null;
    email: string | null;
    address: string | null;
    note: string | null;
    introducerId: number | null;
  };
}