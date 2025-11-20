declare global {
  type Gender = "Male" | "Female";

  type CustomerGetListRequestDto = Partial<{
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
  }>;

  type CustomerGetListResponseDto = {
    items: CustomerBasicResponseDto[];
    pageCount: number;
  };

  type CustomerGetDetailResponseDto = {
    id: string;
    firstName: string;
    middleName: string | null;
    lastName: string;
    nickName: string;
    gender: Gender;
    fullName: string;
    zaloNumber: string | null;
    facebookUrl: string | null;
    email: string | null;
    address: string | null;
    note: string | null;
    createdUserId: string;
    createdDateTime: string;
    lastUpdatedUserId: string | null;
    lastUpdatedDateTime: string | null;
    intrducer: CustomerBasicResponseDto | null;
    authorization: CustomerExistingAuthorizationResponseDto;
  };
}