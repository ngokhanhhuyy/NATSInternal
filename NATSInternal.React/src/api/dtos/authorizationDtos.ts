declare global {
  type UserExistingAuthorizationResponseDto = {
    canChangePassword: boolean;
    canResetPassword: boolean;
    canDelete: boolean;
    canAddToOrRemoveFromRoles: boolean;
  };
}