declare global {
  type UserBasicResponseDto = {
    id: string;
    userName: string;
    roles: RoleBasicResponseDto[];
    authorization: UserExistingAuthorizationResponseDto;
  };

  type RoleBasicResponseDto = {
    id: string;
    name: string;
    displayName: string;
    powerLevel: string;
  };
}