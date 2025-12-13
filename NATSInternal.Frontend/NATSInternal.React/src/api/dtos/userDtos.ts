declare global {
  type UserGetListRequestDto = ImplementsPartial<ISortableAndPageableListRequestDto, {
    sortByAscending: boolean;
    sortByFieldName: string;
    page: number;
    resultsPerPage: number;
    searchContent: string;
    roleId: string;
  }>;

  type UserGetListResponseDto = {
    items: UserBasicResponseDto[];
    pageCount: number;
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