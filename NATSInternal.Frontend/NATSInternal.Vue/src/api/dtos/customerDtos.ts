declare global {
  type Gender = "Male" | "Female";

  type CustomerGetListRequestDto = ImplementsPartial<
      ISearchableListRequestDto &
      IPageableListRequestDto &
      ISortableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    excludedIds: string[];
  }>;

  type CustomerGetListResponseDto = Implements<IPageableListResponseDto<CustomerGetListCustomerResponseDto>, {
    items: CustomerGetListCustomerResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type CustomerGetListCustomerResponseDto = {
    id: string;
    fullName: string;
    nickName: string | null;
    gender: Gender;
    birthday: string | null;
    phoneNumber: string | null;
    debtRemainingAmount: number;
    authorization: CustomerExistingAuthorizationResponseDto;
  };

  type CustomerGetDetailResponseDto = {
    id: string;
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
    createdUser: UserBasicResponseDto;
    createdDateTime: string;
    lastUpdatedUser: UserBasicResponseDto | null;
    lastUpdatedDateTime: string | null;
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
    introducerId: string | null;
  };
}

export { };