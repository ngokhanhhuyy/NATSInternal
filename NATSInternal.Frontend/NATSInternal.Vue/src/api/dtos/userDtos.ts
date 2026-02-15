declare global {
  type UserGetListRequestDto = ImplementsPartial<
      ISearchableListRequestDto &
      IPageableListRequestDto &
      ISortableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    roleId: string;
  }>;

  type UserGetListResponseDto = Implements<IPageableListResponseDto<UserGetListUserResponseDto>, {
    items: UserGetListUserResponseDto[];
    pageCount: number;
    itemCount: number;
  }>;

  type UserGetListUserResponseDto = UserBasicResponseDto & {
    authorization: UserExistingAuthorizationResponseDto;
  };

  type UserGetDetailResponseDto = {
    id: string;
    userName: string;
    roles: UserGetDetailRoleResponseDto[];
    authorization: UserExistingAuthorizationResponseDto
  };

  type UserGetDetailRoleResponseDto = {
    id: string;
    name: string;
    displayName: string;
    powerLevel: number;
    permissionNames: string[];
  };
}

export { };