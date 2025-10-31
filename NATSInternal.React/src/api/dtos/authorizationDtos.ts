declare global {
  type UserExistingAuthorizationResponseDto = {
    canChangePassword: boolean;
    canResetPassword: boolean;
    canDelete: boolean;
    canAddToOrRemoveFromRoles: boolean;
  };
  
  type ProductExistingAuthorizationResponseDto = {
    canEdit: boolean;
    canDelete: boolean;
  }
}