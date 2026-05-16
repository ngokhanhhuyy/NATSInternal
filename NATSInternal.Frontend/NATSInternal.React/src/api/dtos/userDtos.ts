declare global {
  type UserListRequestDto = ImplementsPartial<ISearchableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    roleIds: number[];
  }>;

  type UserListResponseDto = Implements<IListResponseDto<UserBasicResponseDto>, {
    items: UserBasicResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type UserDetailResponseDto = {
    id: number;
    userName: string;
    roles: RoleDetailResponseDto[];
    authorization: UserExistingAuthorizationResponseDto
  };
  
  type UserCreateRequestDto = {
    userName: string;
    password: string;
    confirmationPassword: string;
    roleNames: string[];
  }
  
  type UserUpdateRequestDto = {
    roleIds: number[];
  }
}